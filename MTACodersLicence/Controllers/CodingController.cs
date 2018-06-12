using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.SolutionModels;
using MTACodersLicence.Services;
using MTACodersLicence.ViewModels;
using Newtonsoft.Json.Linq;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class CodingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CodingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private bool IsChallengeActive(ChallengeModel challenge)
        {
            // verificam daca este in desfasurare
            if (challenge?.Contest != null)
            {
                // verificam daca este activata
                if (!challenge.Active)
                    return false;
                // verificam daca a trecut peste timpul admis
                if (challenge.Contest.Duration - (DateTime.Now - challenge.Contest.StartDate).TotalMinutes < 0)
                    return false;
                // verificam daca este inaintea inceperii concursului
                if (DateTime.Now.CompareTo(challenge.Contest.StartDate) < 0)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        public async Task<IActionResult> Index(int? id, string stdout, string stderr, string error, float grade, bool hasGrade)
        {
            if (id == null)
                return NotFound();
            // challenge-ul in curs de desfasurare
            var challenge = _context.Challenges
                                    .Include(s => s.Contest)
                                    .FirstOrDefault(m => m.Id == id);
            if (!IsChallengeActive(challenge))
            {
                return RedirectToAction("EroareTimp");
            }
            //inserare template-uri aferente challenge-ului in limbaje
            var programmingLanguages = _context.ProgrammingLanguages.Where(s => s.Available).ToList();
            var codeTemplates = _context.CodeTemplates.Where(s => s.ChallengeId == id);
            foreach (var codeTemplate in codeTemplates)
            {
                foreach (var language in programmingLanguages)
                {
                    if (codeTemplate.ProgrammingLanguageId == language.Id)
                    {
                        language.CodeTemplate = codeTemplate.Code;
                    }
                }
            }
            //formare sesiune de coding
            var userId = _userManager.GetUserId(User);  // id-ul utilizatorului curent
            var codingSession = _context.CodingSessions     // verificam sa vedem daca este salvata o sesiune deja
                                        .Include(s => s.ProgrammingLanguage)
                                        .FirstOrDefault(s => s.ChallengeId == id && s.ApplicationUserId == userId);
            if (codingSession == null)      // daca nu este una salvata in baza de date cream una aferenta utilizatorului si challenge-ului
            {
                var progLang = programmingLanguages.First();
                var newCodingSession = new CodingSessionModel
                {
                    ChallengeId = challenge.Id,
                    ApplicationUserId = userId,
                    ProgrammingLanguageId = progLang.Id,
                    Code = progLang.CodeTemplate,
                };
                _context.CodingSessions.Add(newCodingSession);
                await _context.SaveChangesAsync();
                codingSession = newCodingSession;
            }
            // creare condingView pentru afisare in view
            var codingViewModel = new CodingViewModel
            {
                Challenge = challenge,
                CodingSession = codingSession,
                HasGrade = hasGrade,
                Grade = grade,
                ProgramingLanguages = programmingLanguages
            };
            // creare select list pentru selectare limbaj de programare
            var currentLangCode = codingSession.ProgrammingLanguage.LanguageCode;
            var programmingLanguagesSelectList = new SelectList(programmingLanguages, "LanguageCode", "Name", currentLangCode);
            ViewData["ProgrammingLanguages"] = programmingLanguagesSelectList;
            //calculare timp ramas din concurs
            var contest = challenge.Contest;
            var remainingTime = contest.Duration - (DateTime.Now - contest.StartDate).TotalMinutes;
            ViewData["remainingTime"] = Math.Round(remainingTime);
            return View(codingViewModel);
        }

        public IActionResult EroareTimp()
        {
            return View();
        }

        private void SaveCodeFunc(string savedCode, int? challengeId, int programmmingLanguageId)
        {
            var userId = _userManager.GetUserId(User);
            var codingSession = _context.CodingSessions
                .FirstOrDefault(s => s.ChallengeId == challengeId && s.ApplicationUserId == userId);
            if (codingSession == null)
            {
                return;
            }
            codingSession.Code = savedCode?.Replace("\\\"", "\"");
            codingSession.ProgrammingLanguageId = programmmingLanguageId;
            codingSession.HasPreviousSave = true;
            _context.CodingSessions.Update(codingSession);
            _context.SaveChanges();
        }

        public IActionResult SaveCode(string savedCode, int? challengeId, string language)
        {
            var programmingLanguage = _context.ProgrammingLanguages.FirstOrDefault(s => s.Name == language);
            if (programmingLanguage != null)
                SaveCodeFunc(savedCode, challengeId, programmingLanguage.Id);
            return RedirectToAction("Index", new { id = challengeId });
        }

        [HttpPost]
        public IActionResult Code([Bind("Code,ChallengeId")] SolutionModel solution, string code, string input, int language, string codeButton, int? challengeId)
        {
            var programmingLanguage = _context.ProgrammingLanguages.FirstOrDefault(s => s.LanguageCode == language);
            if (codeButton.Equals("Submit Code"))
            {
                if (ModelState.IsValid)
                {
                    // salvam codul deja scris pentru restaurare
                    SaveCodeFunc(code, challengeId, programmingLanguage.Id);
                    var userId = _userManager.GetUserId(User);
                    // preluam sesiunea de coding din baza de date
                    var codingSession = _context.CodingSessions
                        .Include(s => s.Challenge)
                            .ThenInclude(s => s.Contest)
                        .FirstOrDefault(s => s.ApplicationUserId == userId && s.ChallengeId == challengeId);
                    if (codingSession == null)
                        return NotFound();
                    // formare solutie
                    solution.ApplicationUserId = userId;
                    solution.ReceiveDateTime = DateTime.Now;
                    solution.ProgrammingLanguageId = programmingLanguage.Id;
                    solution.Duration = DateTime.Now - codingSession.Challenge.Contest.StartDate;
                    _context.Add(solution);
                    _context.SaveChanges();
                    return RedirectToAction("VerifySubmit","Solution", new { id = solution.Id });
                }
            }
            if (codeButton.Equals("Run Tests"))
            {
                SaveCodeFunc(code, challengeId, programmingLanguage.Id);
                var publicBatteries = _context.Batteries
                                                .Where(s => s.IsPublic)
                                                .Include(s => s.Tests)
                                                .AsNoTracking();
                var totalTestsCount = 0;
                var passedTestsCount = 0;
                foreach (var battery in publicBatteries)
                {
                    totalTestsCount += battery.Tests.Count;
                    foreach (var test in battery.Tests)
                    {
                        var codeRunnerResult = CodeRunner.RunCode(code, test, programmingLanguage.LanguageCode);
                        if (codeRunnerResult.PointsGiven > 0)
                        {
                            passedTestsCount++;
                        }
                    }
                }
                var grade = (float)passedTestsCount / totalTestsCount;
                grade = grade * 10;
                return RedirectToAction("Index", new { id = challengeId, grade, hasGrade = true });
            }
            return NotFound();
        }
    }
}