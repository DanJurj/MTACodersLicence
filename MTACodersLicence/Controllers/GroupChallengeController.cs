using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class GroupChallengeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupChallengeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroupChallenge
        public async Task<IActionResult> Index(int? groupId)
        {
            var applicationDbContext = _context.GroupChallengeModel
                                                .Include(g => g.Challenge)
                                                .Include(g => g.Group)
                                                .Where(s => s.GroupId == groupId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GroupChallenge/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupChallengeModel = await _context.GroupChallengeModel
                .Include(g => g.Challenge)
                .Include(g => g.Group)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (groupChallengeModel == null)
            {
                return NotFound();
            }

            return View(groupChallengeModel);
        }

        // GET: GroupChallenge/Create
        public IActionResult Create()
        {
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id");
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
            return View();
        }

        // POST: GroupChallenge/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupId,ChallengeId")] GroupChallengeModel groupChallengeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupChallengeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", groupChallengeModel.ChallengeId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", groupChallengeModel.GroupId);
            return View(groupChallengeModel);
        }

        // GET: GroupChallenge/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupChallengeModel = await _context.GroupChallengeModel.SingleOrDefaultAsync(m => m.Id == id);
            if (groupChallengeModel == null)
            {
                return NotFound();
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", groupChallengeModel.ChallengeId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", groupChallengeModel.GroupId);
            return View(groupChallengeModel);
        }

        // POST: GroupChallenge/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,ChallengeId")] GroupChallengeModel groupChallengeModel)
        {
            if (id != groupChallengeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupChallengeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupChallengeModelExists(groupChallengeModel.Id))
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
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", groupChallengeModel.ChallengeId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", groupChallengeModel.GroupId);
            return View(groupChallengeModel);
        }

        // GET: GroupChallenge/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupChallengeModel = await _context.GroupChallengeModel
                .Include(g => g.Challenge)
                .Include(g => g.Group)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (groupChallengeModel == null)
            {
                return NotFound();
            }

            return View(groupChallengeModel);
        }

        // POST: GroupChallenge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupChallengeModel = await _context.GroupChallengeModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.GroupChallengeModel.Remove(groupChallengeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupChallengeModelExists(int id)
        {
            return _context.GroupChallengeModel.Any(e => e.Id == id);
        }
    }
}
