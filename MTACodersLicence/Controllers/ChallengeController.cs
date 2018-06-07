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
using MTACodersLicence.Models.GroupModels;
using MTACodersLicence.ViewModels;
using Newtonsoft.Json;

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

        private List<ChallengeModel> OrderAndSearch(string searchString, string order, IEnumerable<ChallengeModel> challenges)
        {
            //search
            if (!string.IsNullOrEmpty(searchString))
            {
                return challenges.Where(s => s.Name.Contains(searchString)).ToList();
            }
            //ordonare
            switch (order)
            {
                case "nameAsc": return challenges.OrderBy(s => s.Name).ToList();
                case "nameDesc": return challenges.OrderByDescending(s => s.Name).ToList();
                case "timeAsc": return challenges.OrderBy(s => s.Time).ToList();
                case "timeDesc": return challenges.OrderByDescending(s => s.Time).ToList();
                default: return challenges.ToList();
            }
        }

        // GET: Challenge
        public IActionResult Index(int? contestId, string searchString, string order)
        {
            if (contestId == null)
            {
                return NotFound();
            }
            var challenges = _context.Challenges.Where(s => s.ContestId == contestId);
            ViewData["ContestId"] = contestId;
            return View(challenges.ToList());

            if (User.IsInRole("Administrator"))
            {
                // daca e administrator poate sa vada toate concursurile din baza de date
                var allChallenges = _context.Challenges
                    .Include(s => s.Owner)
                    .ToList();
                allChallenges = OrderAndSearch(searchString, order, allChallenges);
                return View(allChallenges);
            }
            else if (User.IsInRole("Profesor"))
            {
                // daca e profesor poate sa vada toate concursurile create de el
                var challengesProfessor = _context.Challenges
                                        .Include(s => s.Owner)
                                        .Where(c => c.ApplicationUserId == _userManager.GetUserId(User))
                                        .ToList();
                challengesProfessor = OrderAndSearch(searchString, order, challengesProfessor);
                return View(challengesProfessor);
            }
            else
            {
                if (User.IsInRole("Student"))
                {
                    // selectam toate grupurile in care este inscris utilizatorul curent
                    var groups = _context.GroupMembers.Include(s => s.Group)
                        .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                        .Select(s => s.Group)
                        .Select(s => s.Challenges)
                        .ToList();

                    var challengesStudent = new List<ChallengeModel>();
                    foreach (var groupMember in groups)
                    {
                        foreach (var challengeModel in groupMember)
                        {
                            var challenge = _context.Challenges.FirstOrDefault(s => s.Id == challengeModel.ChallengeId);
                            challengesStudent.Add(challenge);
                        }
                    }
                    challengesStudent = OrderAndSearch(searchString, order, challengesStudent);
                    return View(challengesStudent);
                }
                return NotFound();
            }
        }

        // GET: Challenge/Details/5
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

            return View(challengeModel);
        }

        // GET: Challenge/Create
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult Create(int? contestId) 
        {
            if (contestId == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = contestId;
            return View();
        }

        // POST: Challenge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Create([Bind("Name,ShortDescription,Desciption,Tasks,Time,Hint,CodeTemplate")] ChallengeModel challengeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    challengeModel.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Add(challengeModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }
            return View(challengeModel);
        }

        // GET: Challenge/Edit/5
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
            return View(challengeModel);
        }

        // POST: Challenge/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Edit([Bind("Id,Name,ShortDescription,Desciption,Tasks,Time,Hint,ApplicationUserId,Active")] ChallengeModel challengeModel)
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(challengeModel);
        }

        // GET: Challenge/Delete/5
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges
                .Include(c => c.Owner)
                .Include(c => c.Batteries)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }

            return View(challengeModel);
        }

        // POST: Challenge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challengeModel = await _context.Challenges
                                                .Include(m => m.Batteries)
                                                    .ThenInclude(m => m.Tests)
                                                .SingleOrDefaultAsync(m => m.Id == id);
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
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeModelExists(int id)
        {
            return _context.Challenges.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Ranking(int id)
        {
            var solutions = await _context.Solutions.Include(s => s.Owner)
                .Where(s => s.ChallengeId == id).ToListAsync();
            var rankingList = solutions.Select(solution => new RankingViewModel()
                {
                    Grade = solution.Grade,
                    Score = solution.Score,
                    SentBy = solution.Owner.FullName
                })
                .ToList();
            return View(rankingList);
        }
    }
}
