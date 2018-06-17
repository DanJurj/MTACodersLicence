using System;
using System.Linq;
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
using MTACodersLicence.ViewModels;

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
        
        // metoda privata care verifica daca problema este din cadrul unui concurs in desfasurare
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

        /// <summary>
        /// Pagina de concurs a unei probleme. Aici se rezolva problema si se trimite solutia
        /// </summary>
        /// <param name="id">id-ul problemei</param>
        /// <param name="grade">nota obtinuta in urma trimiterii unei solutii</param>
        /// <param name="hasGrade">parametru de verificare in View daca a fost verificata solutia si a primit nota</param>
        public async Task<IActionResult> Index(int? id, float grade, bool hasGrade)
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

        /// <summary>
        /// Pagina afisata daca problema se afla intr-un concurs care s-a terminat
        /// </summary>
        /// <returns>O pagina in care se explica faptul ca acel concurs s-a terminat</returns>
        public IActionResult EroareTimp()
        {
            return View();
        }

        // metoda privata apelata cu scopul de a salva codul pentru a putea reveni ulterior la rezolvarea unei probleme
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

        // Actiune apelata la apasarea butonului de Save sau la CTRL+S
        public IActionResult SaveCode(string savedCode, int? challengeId, string language)
        {
            var programmingLanguage = _context.ProgrammingLanguages.FirstOrDefault(s => s.Name == language);
            if (programmingLanguage != null)
                SaveCodeFunc(savedCode, challengeId, programmingLanguage.Id);
            return RedirectToAction("Index", new { id = challengeId });
        }

        /// <summary>
        /// Actiune apelata in momentul apasarii butonului Submit Code
        /// </summary>
        /// <param name="solution">modelul continant codul sursa si id-ul problemei</param>
        /// <param name="language">LanguageCode-ul limbajului de programare ales</param>
        /// <returns>Pagina de clasament a problemei</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Code([Bind("Code,ChallengeId")] SolutionModel solution, int language)
        {
            if (ModelState.IsValid)
            {
                var programmingLanguage = _context.ProgrammingLanguages.FirstOrDefault(s => s.LanguageCode == language);
                // salvam codul deja scris pentru restaurare
                SaveCodeFunc(solution.Code, solution.ChallengeId, programmingLanguage.Id);
                var userId = _userManager.GetUserId(User);
                // preluam sesiunea de coding din baza de date
                var codingSession = _context.CodingSessions
                    .Include(s => s.Challenge)
                    .ThenInclude(s => s.Contest)
                    .FirstOrDefault(s => s.ApplicationUserId == userId && s.ChallengeId == solution.ChallengeId);
                if (codingSession == null)
                    return NotFound();
                // formare solutie
                solution.ApplicationUserId = userId;
                solution.ReceiveDateTime = DateTime.Now;
                solution.ProgrammingLanguageId = programmingLanguage.Id;
                solution.Duration = DateTime.Now - codingSession.Challenge.Contest.StartDate;
                _context.Add(solution);
                _context.SaveChanges();
                return RedirectToAction("VerifySubmit", "Solution", new { id = solution.Id });
            }           
            return NotFound();
        }
    }
}