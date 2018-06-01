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
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Group
        public async Task<IActionResult> Index()
        {
            // administrator
            var groups = await _context.Groups
                                        .Include(s => s.Owner)
                                        .Include(s => s.Members)
                                        .Include(s => s.Challenges)
                                        .ToListAsync();
            if (User.IsInRole("Profesor"))
            {
                groups = await _context.Groups
                    .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                    .ToListAsync();
            }
            else if (User.IsInRole("Student"))
            {
                groups = await _context.GroupMembers
                    .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                    .Select(s => s.Group)
                    .ToListAsync();
            }
            return View(groups);
        }

        // GET: Group/Create
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Create([Bind("Name")] GroupModel groupModel)
        {
            if (ModelState.IsValid)
            {
                groupModel.ApplicationUserId = _userManager.GetUserId(User);
                _context.Add(groupModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupModel);
        }

        // GET: Group/Edit/5
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupModel = await _context.Groups.SingleOrDefaultAsync(m => m.Id == id);
            if (groupModel == null)
            {
                return NotFound();
            }
            return View(groupModel);
        }

        // POST: Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] GroupModel groupModel)
        {
            if (id != groupModel.Id)
            {
                return NotFound();
            }
            //security measure
            if (ModelState.IsValid)
            {
                try
                {
                    var group = await _context.Groups.SingleOrDefaultAsync(s => s.Id == groupModel.Id);
                    var user = await _context.ApplicationUser
                                            .SingleOrDefaultAsync(s => s.Id == _userManager.GetUserId(User));
                    bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
                    if (group.ApplicationUserId != user.Id && !isAdmin)
                    {
                        return NotFound();
                    }
                    group.Name = groupModel.Name;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupModelExists(groupModel.Id))
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
            return View(groupModel);
        }

        // GET: Group/Delete/5
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupModel = await _context.Groups
                .Include(g => g.Owner)
                .Include(g => g.Challenges)
                    .ThenInclude(s => s.Challenge)
                .Include(g => g.Members)
                    .ThenInclude(s => s.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            // verificare daca este administrator sau daca este profesor si este grupul sau
            if (!User.IsInRole("Administrator") && groupModel.ApplicationUserId!=_userManager.GetUserId(User))
            {
                return NotFound();
            }
            if (groupModel == null)
            {
                return NotFound();
            }

            return View(groupModel);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupModel = await _context.Groups
                                            .Include(s => s.Challenges)
                                            .Include(s => s.Members)
                                            .SingleOrDefaultAsync(m => m.Id == id);
            _context.Groups.Remove(groupModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupModelExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
