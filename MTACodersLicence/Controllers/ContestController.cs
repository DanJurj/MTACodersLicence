using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class ContestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <param name="order">parametrul in functie de care sa se faca sortarea rezultatelor</param>
        /// <returns>Pagina de afisare a concursurilor.</returns>
        public IActionResult Index(string order)
        {
            //administrator
            var contests = _context.Contests
                                    .Include(c => c.Owner)
                                    .Include(s => s.Challenges)
                                    .ToList();
            //daca e profesor poate sa vada doar cele create de el
            if (User.IsInRole("Profesor"))
            {
                contests = contests.Where(s => s.ApplicationUserId == _userManager.GetUserId(User)).ToList();
            }
            //daca e student poate vedea doar concursurile disponibile pentru grupurile in care este inscris
            if (User.IsInRole("Student"))
            {
                var contestsList = _context.GroupMembers.Include(s => s.Group)
                    .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                    .Select(s => s.Group)
                    .Select(s => s.Contests)
                    .ToList();
                contests = new List<ContestModel>();
                foreach (var item in contestsList)
                    foreach (var contestGroup in item)
                        contests.Add(contestGroup.Contest);
            }
            // ordonare in functie de Nume, Data inceperii si durata
            switch (order)
            {
                case "nameAsc":
                    contests = contests.OrderBy(s => s.Name).ToList();
                    break;
                case "nameDesc":
                    contests = contests.OrderByDescending(s => s.Name).ToList();
                    break;
                case "dateAsc":
                    contests = contests.OrderBy(s => s.StartDate).ToList();
                    break;
                case "dateDesc":
                    contests = contests.OrderByDescending(s => s.StartDate).ToList();
                    break;
                case "timeAsc":
                    contests = contests.OrderBy(s => s.Duration).ToList();
                    break;
                case "timeDesc":
                    contests = contests.OrderByDescending(s => s.Duration).ToList();
                    break;
                case "challengesAsc":
                    contests = contests.OrderBy(s => s.Challenges.Count).ToList();
                    break;
                case "challengesDesc":
                    contests = contests.OrderByDescending(s => s.Challenges.Count).ToList();
                    break;
                default: break;
            }
            return View(contests);
        }

        // return: Pagina de Create
        [Authorize(Roles = "Administrator, Profesor")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Profesor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Active")] ContestModel contestModel)
        {
            if (ModelState.IsValid)
            {
                contestModel.ApplicationUserId = _userManager.GetUserId(User);
                _context.Add(contestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contestModel);
        }

        [Authorize(Roles = "Administrator, Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestModel = await _context.Contests.SingleOrDefaultAsync(m => m.Id == id);
            if (contestModel == null)
            {
                return NotFound();
            }
            return View(contestModel);
        }

        [Authorize(Roles = "Administrator, Profesor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Active")] ContestModel contestModel)
        {
            if (id != contestModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contestModel.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Update(contestModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestModelExists(contestModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contestModel);
        }

        /// <summary>
        /// Actiunea apelata pentru stergerea unui concurs. Se vor sterge recursiv si toate problemele cu bateriile de teste si sabloanele aferente.
        /// </summary>
        /// <param name="id">id-ul concursului</param>
        /// <returns>Pagina Index cu respectivul concurs sters in caz de succes</returns>
        [Authorize(Roles = "Administrator, Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestModel = await _context.Contests
                .Include(s => s.Challenges)
                    .ThenInclude(s => s.Batteries)
                        .ThenInclude(s => s.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            foreach (var challenge in contestModel.Challenges)
                foreach (var battery in challenge.Batteries)
                {
                    foreach (var test in battery.Tests)
                        _context.Tests.Remove(test);
                    _context.Batteries.Remove(battery);
                }
            // daca nu e administrator si concursul nu e creat de el, atunci nu-l poate sterge
            if (!User.IsInRole("Administrator") && _userManager.GetUserId(User) != contestModel.ApplicationUserId)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Contests.Remove(contestModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContestModelExists(int id)
        {
            return _context.Contests.Any(e => e.Id == id);
        }

        //clasamentul total per concurs
        public IActionResult Ranking(int id)
        {
            var challenges = _context.Challenges
                .Include(s => s.Solutions)
                .Where(s => s.ContestId == id)
                .ToList();

            var solutions = _context.Solutions
                .Include(s => s.ProgrammingLanguage)
                .Include(s => s.Owner)
                .Include(s => s.Challenge)
                .Where(s => s.Challenge.ContestId == id)
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
