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
using MTACodersLicence.ViewModels.GroupViewModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class GroupMemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupMemberController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GroupMember
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Index(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }

            var groupMembers = _context.GroupMembers
                                    .Include(g => g.Group)
                                    .Include(g => g.User)
                                    .Where(s => s.GroupId == groupId);
            ViewData["GroupId"] = groupId;
            return View(await groupMembers.ToListAsync());
        }

        // GET: GroupMember/Details/5
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Details(int? id, int? groupId)
        {
            if (id == null || groupId == null)
            {
                return NotFound();
            }

            var groupMemberModel = await _context.GroupMembers
                .Include(g => g.Group)
                .Include(g => g.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            ViewData["GroupId"] = groupId;
            if (groupMemberModel == null)
            {
                return NotFound();
            }

            return View(groupMemberModel);
        }

        // GET: GroupMember/Create
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult Create(int? groupId)
        {
            ViewData["GroupId"] = groupId;
            var users = _context.ApplicationUser.ToList();
            var members = _context.GroupMembers.Where(s => s.GroupId == groupId).ToList();
            foreach (var member in members)
            {
                users.Remove(member.User);
            }
            var groupMemberViewModel = new GroupMemberViewModel
            {
                Users = users,
                Members = members
            };
            return View(groupMemberViewModel);
        }

        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> AddMember(string userId, int? groupId)
        {
            if (userId == null || groupId == null)
            {
                return NotFound();
            }
            var groupMember = new GroupMemberModel
            {
                ApplicationUserId = userId,
                GroupId = (int) groupId,
                JoinDate = DateTime.Now
            };
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create), new { groupId });
        }

        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> RemoveMember(string userId, int? groupId)
        {
            if (userId == null || groupId == null)
            {
                return NotFound();
            }

            var groupMember = _context.GroupMembers.Single(s => s.ApplicationUserId == userId && s.GroupId == groupId);
            if (groupMember == null)
                return RedirectToAction(nameof(Create), new {groupId});
            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create), new { groupId });
        }

        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> RemoveMemberPrim(string userId, int? groupId)
        {
            if (userId == null || groupId == null)
            {
                return NotFound();
            }

            var groupMember = _context.GroupMembers.Single(s => s.ApplicationUserId == userId && s.GroupId == groupId);
            if (groupMember == null)
                return RedirectToAction(nameof(Index), new { groupId });
            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> LeaveGroup(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }
            var groupMember = _context.GroupMembers
                                        .Single(s => s.ApplicationUserId == _userManager.GetUserId(User) && s.GroupId == groupId);
            if (groupMember == null)
                return RedirectToAction("Index", "Group");
            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Group");
        }
    }
}
