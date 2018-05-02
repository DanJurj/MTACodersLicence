using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using System.Security.Claims;

namespace MTACodersLicence.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext _ctx;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MyGroups()
        {
            var groups = _ctx.Groups.Include( s => s.Items)
                                    .AsNoTracking();
            return View(groups);
        }


        public IActionResult EditGroup(int id)
        {
            var group = _ctx.Groups.Where(b => b.Id == id).Include("Items").FirstOrDefault();
            return View(group);
        }


        [HttpGet]
        public async Task<IActionResult> EditMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var memberToUpdate = await _ctx.GroupItems.FirstOrDefaultAsync(b => b.Id == id);
            return View(memberToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(int id)
        {
            var memberToUpdate = await _ctx.GroupItems.FirstOrDefaultAsync(b => b.Id == id);
            if (await TryUpdateModelAsync<GroupItem>(memberToUpdate,"",
                s => s.FirstName, s => s.LastName, s => s.UserName, s=>s.EMail, s=> s.AddeDateTime))
            {
                try
                {
                    await _ctx.SaveChangesAsync();
                    return RedirectToAction(nameof(MyGroups));
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return View(memberToUpdate);
        }






        [HttpGet]
        public IActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup(
            [Bind("Name")] GroupModel group)
        {
            if (ModelState.IsValid)
            {
                group.ApplicationUserId = _userManager.GetUserId(User);
                _ctx.Groups.Add(group);
                await _ctx.SaveChangesAsync();
                return RedirectToAction(nameof(MyGroups));
            }
            return View();
        }


        [HttpGet]
        public IActionResult CreateMember(string groupName)
        {
            ViewData["groupName"] = groupName;
            return View();
        }

        [HttpPost]
        public IActionResult CreateMember(GroupItem member, string groupName)
        {
            GroupModel group = _ctx.Groups.First(s => s.Name.Equals(groupName));
            member.GroupId = group.Id;
            if (group.Items == null)
            {
                group.Items = new List<GroupItem>();
            }
            group.Items.Add(member);
            _ctx.SaveChanges();
            return View();
        }

        public async Task<IActionResult> GroupDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _ctx.Groups.Include(s => s.Items).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        public async Task<IActionResult> DeleteGroup(int? id)
        {
            var group = await _ctx.Groups.Include("Items").FirstOrDefaultAsync(s => s.Id == id);
            _ctx.Entry(group).State = EntityState.Deleted;
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(MyGroups));
        }

        public async Task<IActionResult> DeleteMember(int? id)
        {
            try
            {
                var memberToDelete = await _ctx.GroupItems.FirstOrDefaultAsync(s => s.Id == id);
                _ctx.Entry(memberToDelete).State = EntityState.Deleted;
                await _ctx.SaveChangesAsync();
                return RedirectToAction(nameof(MyGroups));
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Error));
            }
            
        }
    }
}
