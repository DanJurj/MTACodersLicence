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

        // GET: Challenge
        public async Task<IActionResult> Index(string searchString, string type, string sortOrder)
        {
            var challenges = _context.Challenges.Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
            if (!String.IsNullOrEmpty(type))
            {
                if (type.Equals("my"))
                {
                    challenges =
                        _context.Challenges.Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
                }
                else if (type.Equals("all"))
                {
                    challenges =
                        _context.Challenges.Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
                }
            }
            //search
            if (!String.IsNullOrEmpty(searchString))
            {
                challenges = challenges.Where(s => s.Name.Contains(searchString));
            }
            //ordonare
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "time_desc" : "Time";
            switch (sortOrder)
            {
                case "name_desc":
                    challenges = challenges.OrderByDescending(s => s.Name);
                    break;
                case "Time":
                    challenges = challenges.OrderBy(s => s.Time);
                    break;
                case "time_desc":
                    challenges = challenges.OrderByDescending(s => s.Time);
                    break;
                default:
                    challenges = challenges.OrderBy(s => s.Name);
                    break;
            }

            return View(await challenges.AsNoTracking().ToListAsync());
        }

        // GET: Challenge/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges
                .Include(c => c.ApplicationUser)
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Challenge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ShortDescription,Desciption,Tasks,Time,Hint")] ChallengeModel challengeModel)
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,ShortDescription,Desciption,Tasks,Time,Hint,ApplicationUserId")] ChallengeModel challengeModel)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges
                .Include(c => c.ApplicationUser)
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
    }
}
