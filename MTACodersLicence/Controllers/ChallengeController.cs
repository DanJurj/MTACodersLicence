using System;
using System.Collections.Generic;
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
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class ChallengeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChallengeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Folosit de profesor in general pentru a adauga/edita/sterge/modifica problemele unui concurs dar si
        /// si pentru a vedea solutiile trimise pentru aceasta
        /// </summary>
        /// <param name="contestId">id-ul concursului</param>
        /// <param name="order">in functie de ce coloana sa se ordoneze rezultatele</param>
        [Authorize(Roles = "Administrator, Profesor")]
        public IActionResult Index(int? contestId, string order)
        {
            if (contestId == null)
            {
                return NotFound();
            }
            var challenges = _context.Challenges
                                    .Include(s => s.Owner)
                                    .Where(s => s.ContestId == contestId)
                                    .ToList();
            // ordonare
            switch (order)
            {
                case "nameAsc":
                    challenges = challenges.OrderBy(s => s.Name).ToList();
                    break;
                case "nameDesc":
                    challenges = challenges.OrderByDescending(s => s.Name).ToList();
                    break;
            }
            ViewData["ContestId"] = contestId;
            return View(challenges.ToList());
        }

        /// <summary>
        /// Actiune apelata de studenti in momentul in care vor sa inceapa un concurs
        /// </summary>
        /// <param name="contestId">id-ul concursului</param> 
        public IActionResult Start(int? contestId)
        {
            if (contestId == null)
            {
                return NotFound();
            }
            var challenges = _context.Challenges.Where(s => s.ContestId == contestId);
            ViewData["ContestId"] = contestId;
            var contest = _context.Contests.FirstOrDefault(s => s.Id == contestId);
            if (contest != null)
            {
                var remainingTime = contest.Duration - (DateTime.Now - contest.StartDate).TotalMinutes;
                ViewData["remainingTime"] = Math.Round(remainingTime);
            }
            return View(challenges.ToList());
        }

        /// <param name="id">id-ul problemei</param>
        /// <returns>Pagina cu Detaliile unei probleme incluzand si bateriile de teste cu testele aferente</returns>
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges
                .Include(c => c.Owner)
                .Include(c => c.Batteries)
                    .ThenInclude(c => c.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = challengeModel.ContestId;
            return View(challengeModel);
        }

        /// <param name="contestId">id-ul concursului pentru care se creaza problema aceasta</param>
        /// <returns>Pagina de creare a unei probleme</returns>
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult Create(int? contestId)
        {
            if (contestId == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = contestId;
            var dificulties = new List<SelectListItem>
            {
                new SelectListItem {Text = "Easy", Value = "Easy"},
                new SelectListItem {Text = "Medium", Value = "Medium"},
                new SelectListItem {Text = "Hard", Value = "Hard"}
            };
            ViewData["Dificulties"] = dificulties;
            return View();
        }

        /// <summary>
        /// Actiunea apelata in momentul submit-ului de date pentru crearea unei probleme
        /// </summary>
        /// <param name="challengeModel">modelul continand toate datele din form</param>
        /// <returns>Pagina de Index a problemelor in caz de succes sau aceeasi pagina in cazul unei erori</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Create(ChallengeModel challengeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    challengeModel.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Add(challengeModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { contestId = challengeModel.ContestId });
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }
            ViewData["ContestId"] = challengeModel.ContestId;
            return View(challengeModel);
        }

        /// <param name="id">id-ul problemei</param>
        /// <returns>Pagina de editare a unei probleme</returns>
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges.SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = challengeModel.ContestId;
            var dificulties = new List<SelectListItem>
            {
                new SelectListItem {Text = "Easy", Value = "Easy"},
                new SelectListItem {Text = "Medium", Value = "Medium"},
                new SelectListItem {Text = "Hard", Value = "Hard"}
            };
            var dificulty = challengeModel.Dificulty;
            foreach (var item in dificulties)
                if (item.Value.Equals(dificulty))
                {
                    item.Selected = true;
                }
            ViewData["Dificulties"] = dificulties;
            return View(challengeModel);
        }

        /// <summary>
        /// Actiunea apelata in momentul trimiterii datelor modificate
        /// </summary>
        /// <param name="challengeModel"></param>
        /// <returns>Pagina Index in caz de succes sau pagina de Edit in caz de esec</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Edit(ChallengeModel challengeModel)
        {
            challengeModel.ApplicationUserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(challengeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeModelExists(challengeModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new { contestId = challengeModel.ContestId });
            }
            return View(challengeModel);
        }

        /// <summary>
        /// Stergerea unei probleme. Se vor sterge si bateriile de teste impreuna cu testele aferente
        /// </summary>
        /// <param name="id">id-ul problemei</param>
        /// <returns>Index in caz de succes sau NotFound in caz de eroare</returns>
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var challengeModel = await _context.Challenges
                .Include(c => c.Batteries)
                    .ThenInclude(m => m.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }
            var contestId = challengeModel.ContestId;
            var batteries = _context.Batteries.Where(m => m.ChallengeId == id);
            foreach (var battery in batteries)
            {
                var tests = _context.Tests.Where(m => m.BatteryId == battery.Id);
                foreach (var test in tests)
                {
                    _context.Tests.Remove(test);
                }
                _context.Batteries.Remove(battery);
            }
            _context.Challenges.Remove(challengeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { contestId });
        }

        // Verifica existenta problemei cu id-ul id. Folosita la cereri de Edit sau Delete
        private bool ChallengeModelExists(int id)
        {
            return _context.Challenges.Any(e => e.Id == id);
        }

        /// <summary>
        /// Creeaza o lista de rezultate pe baza solutiilor pentru o problema si o trimite spre afisare
        /// </summary>
        /// <param name="id">id-ul problemei</param>
        public IActionResult Ranking(int id)
        {
            var solutions = _context.Solutions
                .Include(s => s.ProgrammingLanguage)
                .Include(s => s.Owner)
                .Where(s => s.ChallengeId == id)
                .ToList();
            var rankingList = solutions.Select(solution => new RankingViewModel()
            {
                Grade = solution.Grade,
                Score = solution.Score,
                SentBy = solution.Owner.FullName,
                TotalExecutionTime = solution.ExecutionTime,
                TotalMemoryUsed = solution.MemoryUsed,
                Language = solution.ProgrammingLanguage.Name
            })
                .ToList().OrderByDescending(s => s.Score)
                .ThenBy(s => s.TotalExecutionTime)
                .ThenBy(s => s.TotalMemoryUsed);
            return View(rankingList);
        }
    }
}
