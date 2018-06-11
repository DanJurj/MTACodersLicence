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

        public async Task<IActionResult> Index(int? id, string stdout, string stderr, string error, float grade, bool hasGrade)
        {
            if (id == null)
            {
                return NotFound();
            }
            var challenge = await _context.Challenges.FirstOrDefaultAsync(m => m.Id == id);
            var progLang = await _context.ProgrammingLanguages.FirstOrDefaultAsync();
            if (challenge == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var codingSession = _context.CodingSessions
                                        .FirstOrDefault(s => s.ChallengeId == id && s.ApplicationUserId == userId);
            // daca utilizatorul are deja o sesiune de coding creata verificam daca nu i-a expirat timpul alocat acesteia
            if (codingSession != null)
            {
                var passedTimee = DateTime.Now - codingSession.StartTime;
                var passedTimeMinutess = passedTimee.TotalMinutes;
                if (passedTimeMinutess > challenge.Time)
                {
                    return RedirectToAction("EroareTimp");
                }
            }

            // daca nu e nici o sesiune de coding inceputa cream una
            if (codingSession == null)
            {
                var newCodingSession = new CodingSessionModel
                {
                    ChallengeId = challenge.Id,
                    StartTime = DateTime.Now,
                    ApplicationUserId = userId,
                    ProgrammingLanguageId = progLang.Id,
                    Code = progLang.CodeTemplate,
                };
                _context.CodingSessions.Add(newCodingSession);
                await _context.SaveChangesAsync();
                codingSession = newCodingSession;
            }
            // cream coding ViewModel-ul
            var codingViewModel = new CodingViewModel
            {
                Challenge = challenge,
                CodingSession = codingSession
            };
            // calculam timpul ramas din challenge
            var passedTime = DateTime.Now - codingSession.StartTime;
            var passedTimeMinutes = passedTime.TotalMinutes;
            codingViewModel.RemainingTime = challenge.Time - (int)passedTimeMinutes;
            codingViewModel.HasGrade = hasGrade;
            codingViewModel.Grade = grade;
           /* if (stdout != null || stderr != null || error != null)
            {
                var codeResult = new CodeRunnerResult()
                {
                    Stdout = stdout,
                    Stderr = stderr,
                    Error = error,
                };
                if (error != null)
                {
                    codeResult.HasError = true;
                }
                codingViewModel.CodeResult = codeResult;
            }*/
            var programmingLanguages = _context.ProgrammingLanguages.Where(s => s.Available).ToList();
            var programmingLanguage = _context.ProgrammingLanguages.FirstOrDefault(s => s.Id == codingSession.ProgrammingLanguageId);
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
            var programmingLanguagesSelectList = new SelectList(programmingLanguages, "LanguageCode", "Name", programmingLanguage.LanguageCode);
            codingViewModel.ProgramingLanguages = programmingLanguages;
            ViewData["ProgrammingLanguages"] = programmingLanguagesSelectList;
            return View(codingViewModel);
        }

        public IActionResult EroareTimp()
        {
            return View();
        }

        private async Task SaveCodeFunc(string savedCode, int? challengeId, int programmmingLanguageId)
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
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> SaveCode(string savedCode, int? challengeId, string language)
        {
            var programmingLanguage = await _context.ProgrammingLanguages.FirstOrDefaultAsync(s => s.Name == language);
            await SaveCodeFunc(savedCode, challengeId, programmingLanguage.Id);
            return RedirectToAction("Index", new { id = challengeId });
        }

        [HttpPost]
        public async Task<IActionResult> Code([Bind("Code,ChallengeId")] SolutionModel solution, string code, string input, int language, string codeButton, int? challengeId)
        {
            var programmingLanguage = await _context.ProgrammingLanguages.FirstOrDefaultAsync(s => s.LanguageCode == language);
            if (codeButton.Equals("Submit Code"))
            {
                if (ModelState.IsValid)
                {
                    await SaveCodeFunc(code, challengeId, programmingLanguage.Id);
                    var userId = _userManager.GetUserId(User);
                    var codingSession = _context.CodingSessions.FirstOrDefault(s =>
                        s.ApplicationUserId == userId && s.ChallengeId == challengeId);
                    solution.ApplicationUserId = userId;
                    solution.ReceiveDateTime = DateTime.Now;
                    solution.ProgrammingLanguageId = programmingLanguage.Id;
                    var existentSolution = await _context.Solutions
                        .FirstOrDefaultAsync(s => s.ApplicationUserId == userId && s.ChallengeId == challengeId);
                    if (existentSolution != null)
                    {
                        existentSolution.Duration = DateTime.Now - codingSession.StartTime;
                        existentSolution.Code = code;
                    }
                    else
                    {
                        solution.Duration = DateTime.Now - codingSession.StartTime;
                        _context.Add(solution);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = solution.ChallengeId });
                }
            }
            if (codeButton.Equals("Run Tests"))
            {
                await SaveCodeFunc(code, challengeId, programmingLanguage.Id);
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