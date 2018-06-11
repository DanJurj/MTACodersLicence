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
    public class GroupContestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GroupContestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GroupChallenge
        public IActionResult Index(int? groupId)
        {
            var availableChallenges =  _context.Challenges
                                             .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                                             .ToList();
            var assignedChallenges =  _context.GroupContests
                                            .Include(g => g.Contest)
                                            .Where(s => s.GroupId == groupId)
                                            .ToList();
            // eliminam din cele available pe cele deja assigned
            foreach (var assignedChallenge in assignedChallenges)
            {
                //availableChallenges.Remove(assignedChallenge.Contest);
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
                ContestId = (int)challengeId,
                AssignDate = DateTime.Now
            };
            _context.GroupContests.Add(groupChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        public async Task<IActionResult> RemoveChallenge(int? groupId, int? challengeId)
        {
            if (groupId == null || challengeId == null)
            {
                return NotFound();
            }
            var groupChallenge = await _context.GroupContests
                                        .SingleOrDefaultAsync(s => s.ContestId == challengeId && s.GroupId == groupId);
            if (groupChallenge == null)
            {
                return NotFound();
            }
            _context.GroupContests.Remove(groupChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        public async Task<IActionResult> AssignedChallenges(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }
            var assignedChallenges = await _context.GroupContests
                .Include(g => g.Contest)
                .Where(s => s.GroupId == groupId)
                .ToListAsync();
            return View(assignedChallenges);
        }

        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> ToogleActivation(int? challengeId, int? groupId, int? active)
        {
            if (challengeId == null || groupId == null || active == null)
            {
                return NotFound();
            }
            var challenge = await _context.Challenges
                                    .FirstOrDefaultAsync(s => s.Id == challengeId);
            if (active == 0)
                challenge.Active = false;
            else
                challenge.Active = true;
            _context.Challenges.Update(challenge);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { groupId });
        }
    }
}
