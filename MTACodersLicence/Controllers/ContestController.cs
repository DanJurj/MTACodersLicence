using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Controllers
{
    public class ContestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Contest
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contests.Include(c => c.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contest/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Time,Active")] ContestModel contestModel)
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

        // GET: Contest/Edit/5
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

        // POST: Contest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Time,Active")] ContestModel  contestModel)
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

        // GET: Contest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contestModel = await _context.Contests
                .SingleOrDefaultAsync(m => m.Id == id);
            if (contestModel == null)
            {
                return NotFound();
            }
            _context.Contests.Remove(contestModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContestModelExists(int id)
        {
            return _context.Contests.Any(e => e.Id == id);
        }

        public IActionResult Ranking()
        {
            throw new NotImplementedException();
        }
    }
}
