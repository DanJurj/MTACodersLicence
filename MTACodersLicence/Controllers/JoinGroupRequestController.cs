using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class JoinGroupRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JoinGroupRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JoinGroupRequest
        public async Task<IActionResult> Index(int? groupId)
        {
            var applicationDbContext = _context.JoinGroupRequests
                                                .Include(j => j.Group)
                                                .Include(j => j.Solicitator)
                                                .Where(s => s.GroupId == groupId);
            var group = _context.Groups.FirstOrDefault(s => s.Id == groupId);
            if (group != null)
            {
                ViewData["GroupName"] = group.Name;
            }
            ViewData["GroupId"] = groupId;
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> AcceptJoinRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await _context.JoinGroupRequests
                .FirstOrDefaultAsync(s => s.Id == id);
            var groupMember = new GroupMemberModel
            {
                GroupId = request.GroupId,
                ApplicationUserId = request.ApplicationUserId,
                JoinDate = DateTime.Now
            };
            _context.GroupMembers.Add(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DeleteJoinRequest), new { id });
        }

        public async Task<IActionResult> DeleteJoinRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = await _context.JoinGroupRequests
                                        .FirstOrDefaultAsync(s => s.Id == id);
            var groupId = request.GroupId;
            _context.JoinGroupRequests.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }

        public async Task<IActionResult> AcceptAll(int? groupId)
        {
            if (groupId == null)
            {
                return NotFound();
            }
            var requests = await _context.JoinGroupRequests
                                        .Where(s => s.GroupId == groupId)
                                        .ToListAsync();
            foreach (var request in requests)
            {
                _context.GroupMembers.Add(new GroupMemberModel()
                {
                    GroupId = (int)groupId,
                    ApplicationUserId = request.ApplicationUserId,
                    JoinDate = DateTime.Now
                });
                _context.JoinGroupRequests.Remove(request);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { groupId });
        }
    }
}
