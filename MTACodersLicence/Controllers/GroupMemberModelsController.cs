using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.Controllers
{
    public class GroupMemberModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupMemberModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GroupMemberModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GroupMembers.Include(g => g.Group).Include(g => g.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GroupMemberModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMemberModel = await _context.GroupMembers
                .Include(g => g.Group)
                .Include(g => g.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (groupMemberModel == null)
            {
                return NotFound();
            }

            return View(groupMemberModel);
        }

        // GET: GroupMemberModels/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: GroupMemberModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupId,ApplicationUserId,JoinDate")] GroupMemberModel groupMemberModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupMemberModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", groupMemberModel.GroupId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", groupMemberModel.ApplicationUserId);
            return View(groupMemberModel);
        }

        // GET: GroupMemberModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMemberModel = await _context.GroupMembers.SingleOrDefaultAsync(m => m.Id == id);
            if (groupMemberModel == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", groupMemberModel.GroupId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", groupMemberModel.ApplicationUserId);
            return View(groupMemberModel);
        }

        // POST: GroupMemberModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,ApplicationUserId,JoinDate")] GroupMemberModel groupMemberModel)
        {
            if (id != groupMemberModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupMemberModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupMemberModelExists(groupMemberModel.Id))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", groupMemberModel.GroupId);
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", groupMemberModel.ApplicationUserId);
            return View(groupMemberModel);
        }

        // GET: GroupMemberModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMemberModel = await _context.GroupMembers
                .Include(g => g.Group)
                .Include(g => g.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (groupMemberModel == null)
            {
                return NotFound();
            }

            return View(groupMemberModel);
        }

        // POST: GroupMemberModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupMemberModel = await _context.GroupMembers.SingleOrDefaultAsync(m => m.Id == id);
            _context.GroupMembers.Remove(groupMemberModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupMemberModelExists(int id)
        {
            return _context.GroupMembers.Any(e => e.Id == id);
        }
    }
}
