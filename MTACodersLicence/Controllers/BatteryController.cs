using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Controllers
{
    public class BatteryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BatteryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Battery
        public async Task<IActionResult> Index(int? challengeId)
        {
            if (challengeId == null)
            {
                return NotFound();
            }
            var batteries = _context.Batteries.
                                    Include(b => b.Challenge).
                                    Include(b => b.Tests).
                                    Where(c => c.ChallengeId == challengeId);
            var challenge = _context.Challenges.FirstOrDefaultAsync(c => c.Id == challengeId);
            if (challenge != null)
            {
                string challengeName = challenge.Result.Name;
                ViewData["challengeName"] = challengeName;
                ViewData["challengeId"] = challengeId;
            }
            return View(await batteries.ToListAsync());
        }

        // GET: Battery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryModel = await _context.Batteries
                .Include(b => b.Challenge)
                .Include(b => b.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (batteryModel == null)
            {
                return NotFound();
            }

            return View(batteryModel);
        }

        // GET: Battery/Create
        public IActionResult Create(int? challengeId)
        {
            ViewData["ChallengeId"] = challengeId;
            return View();
        }

        // POST: Battery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsPublic,ChallengeId")] BatteryModel batteryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batteryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {challengeId = batteryModel.ChallengeId});
            }
            return View(batteryModel);
        }

        // GET: Battery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryModel = await _context.Batteries
                                            .Include(m => m.Tests)
                                            .SingleOrDefaultAsync(m => m.Id == id);
            if (batteryModel == null)
            {
                return NotFound();
            }
            return View(batteryModel);
        }

        // POST: Battery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsPublic,ChallengeId")] BatteryModel batteryModel)
        {
            if (id != batteryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batteryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatteryModelExists(batteryModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new {challengeId = batteryModel.ChallengeId});
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", batteryModel.ChallengeId);
            return View(batteryModel);
        }

        // GET: Battery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryModel = await _context.Batteries
                .Include(b => b.Challenge)
                .Include(b => b.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (batteryModel == null)
            {
                return NotFound();
            }

            return View(batteryModel);
        }

        // POST: Battery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tests = _context.Tests.Where(b => b.BatteryId == id);
            var batteryModel = await _context.Batteries
                                            .Include(m =>m.Tests)
                                            .SingleOrDefaultAsync(m => m.Id == id);
            var challengeId = batteryModel.ChallengeId;
            foreach (var test in tests)
            {
                _context.Tests.Remove(test);
            }
            _context.Batteries.Remove(batteryModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { challengeId });
        }

        private bool BatteryModelExists(int id)
        {
            return _context.Batteries.Any(e => e.Id == id);
        }
    }
}
