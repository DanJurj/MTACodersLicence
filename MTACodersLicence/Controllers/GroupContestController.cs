using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Pagina cu concursurile disponibile si cele atribuite grupului
        /// Profesorul care a creat grupul si concursurile le poate adauga sau nu grupului
        /// </summary>
        /// <param name="groupId">id-ul grupului</param>
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult Index(int? groupId)
        {
            var availableContests =  _context.Contests
                                             .Where(s => s.ApplicationUserId == _userManager.GetUserId(User))
                                             .ToList();
            var assignedContests =  _context.GroupContests
                                            .Include(g => g.Contest)
                                            .Where(s => s.GroupId == groupId)
                                            .ToList();
            // eliminam din cele available pe cele deja assigned
            foreach (var assignedChallenge in assignedContests)
            {
                availableContests.Remove(assignedChallenge.Contest);
            }
            var groupContestViewModel = new GroupContestViewModel
            {
                AssignedContests = assignedContests,
                AvailableContests = availableContests
            };
            ViewData["GroupId"] = groupId;
            return View(groupContestViewModel);
        }

        /// <summary>
        /// Adaugarea unui concurs la un grup. Acum membrii grupului pot vedea concursul
        /// </summary>
        /// <param name="groupId">id-ul grupului</param>
        /// <param name="contestId">id-ul concursului</param>
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> AddContests(int? groupId, int? contestId)
        {
            if (groupId == null || contestId == null)
            {
                return NotFound();
            }
            var groupChallenge = new GroupContestModel
            {
                GroupId = (int)groupId,
                ContestId = (int)contestId,
                AssignDate = DateTime.Now
            };
            _context.GroupContests.Add(groupChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        /// <summary>
        /// Face inaccesibil concursul pentru grupul in cauza
        /// </summary>
        /// <param name="groupId">id-ul grupului</param>
        /// <param name="contestId">id-ul concursului</param>
        /// <returns>Pagina Index</returns>
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> RemoveContest(int? groupId, int? contestId)
        {
            if (groupId == null || contestId == null)
            {
                return NotFound();
            }
            var groupChallenge = await _context.GroupContests
                                        .SingleOrDefaultAsync(s => s.ContestId == contestId && s.GroupId == groupId);
            if (groupChallenge == null)
            {
                return NotFound();
            }
            _context.GroupContests.Remove(groupChallenge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        // actiune disponibila si studentilor pentru a vedea concursurile disponibile grupului respectiv
        public async Task<IActionResult> AssignedContests(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }
            var assignedContests = await _context.GroupContests
                .Include(g => g.Contest)
                .Where(s => s.GroupId == groupId)
                .ToListAsync();
            return View(assignedContests);
        }

        // activarea sau dezactivarea concuruslui. Daca un concurs este atribuit si nu este activat, membrii grupului
        // nu vor putea participa
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> ToogleActivation(int? contestId, int? groupId, int? active)
        {
            if (contestId == null || groupId == null || active == null)
            {
                return NotFound();
            }
            var contest = await _context.Contests
                                    .FirstOrDefaultAsync(s => s.Id == contestId);
            if (active == 0)
                contest.Active = false;
            else
                contest.Active = true;
            _context.Contests.Update(contest);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { groupId });
        }
    }
}
