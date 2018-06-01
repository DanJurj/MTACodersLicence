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
    public class GroupChallengeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupChallengeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GroupChallenge
        public async Task<IActionResult> Index(int? groupId)
        {
            var availableChallenges = await _context.Challenges
                                             .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                                             .ToListAsync();
            var assignedChallenges = await _context.GroupChallenges
                                            .Include(g => g.Challenge)
                                            .Where(s => s.GroupId == groupId)
                                            .ToListAsync();
            // eliminam din cele available pe cele deja assigned
            foreach (var assignedChallenge in assignedChallenges)
            {
                availableChallenges.Remove(assignedChallenge.Challenge);
            }
            var groupChallengeViewModel = new GroupChallengeViewModel
            {
                AssignedChallenges = assignedChallenges,
                AvailableChallenges = availableChallenges
            };
            ViewData["GroupId"] = groupId;
            return View(groupChallengeViewModel);
        }

        public async Task<IActionResult> AddChallenge(int? groupId, int? challengeId)
        {
            if (groupId == null || challengeId == null)
            {
                return NotFound();
            }
            var groupChallenge = new GroupChallengeModel
            {
                GroupId = (int)groupId,
                ChallengeId = (int)challengeId,
                AssignDate = DateTime.Now
            };
            _context.GroupChallenges.Add(groupChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        public async Task<IActionResult> RemoveChallenge(int? groupId, int? challengeId)
        {
            if (groupId == null || challengeId == null)
            {
                return NotFound();
            }
            var groupChallenge = await _context.GroupChallenges
                                        .SingleOrDefaultAsync(s => s.ChallengeId == challengeId && s.GroupId == groupId);
            if (groupChallenge == null)
            {
                return NotFound();
            }
            _context.GroupChallenges.Remove(groupChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        public async Task<IActionResult> AssignedChallenges(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }
            var assignedChallenges = await _context.GroupChallenges
                .Include(g => g.Challenge)
                .Where(s => s.GroupId == groupId)
                .ToListAsync();
            return View(assignedChallenges);
        }
    }
}
