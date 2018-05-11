using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using Newtonsoft.Json;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class ChallengeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChallengeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Challenge
        public async Task<IActionResult> Index(string searchString, string type, string sortOrder)
        {
            var challenges = _context.Challenges.Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
            if (!String.IsNullOrEmpty(type))
            {
                if (type.Equals("my"))
                {
                    challenges =
                        _context.Challenges.Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
                }
                else if (type.Equals("all"))
                {
                    challenges =
                        _context.Challenges.Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
                }
            }
            //search
            if (!String.IsNullOrEmpty(searchString))
            {
                challenges = challenges.Where(s => s.Name.Contains(searchString));
            }
            //ordonare
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "time_desc" : "Time";
            switch (sortOrder)
            {
                case "name_desc":
                    challenges = challenges.OrderByDescending(s => s.Name);
                    break;
                case "Time":
                    challenges = challenges.OrderBy(s => s.Time);
                    break;
                case "time_desc":
                    challenges = challenges.OrderByDescending(s => s.Time);
                    break;
                default:
                    challenges = challenges.OrderBy(s => s.Name);
                    break;
            }

            return View(await challenges.AsNoTracking().ToListAsync());
        }

        // GET: Challenge/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges
                .Include(c => c.ApplicationUser)
                .Include(c => c.Batteries)
                    .ThenInclude(c => c.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }

            return View(challengeModel);
        }

        // GET: Challenge/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Challenge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ShortDescription,Desciption,Tasks,Time,Hint")] ChallengeModel challengeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    challengeModel.ApplicationUserId = _userManager.GetUserId(User);
                    _context.Add(challengeModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
            }
            return View(challengeModel);
        }

        // GET: Challenge/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges.SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }
            return View(challengeModel);
        }

        // POST: Challenge/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,ShortDescription,Desciption,Tasks,Time,Hint,ApplicationUserId")] ChallengeModel challengeModel)
        {
            challengeModel.ApplicationUserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(challengeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeModelExists(challengeModel.Id))
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
            return View(challengeModel);
        }

        // GET: Challenge/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeModel = await _context.Challenges
                .Include(c => c.ApplicationUser)
                .Include(c => c.Batteries)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeModel == null)
            {
                return NotFound();
            }

            return View(challengeModel);
        }

        // POST: Challenge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var challengeModel = await _context.Challenges.SingleOrDefaultAsync(m => m.Id == id);
            var batteries = _context.Batteries.Where(m => m.ChallengeId == id);
            foreach (var battery in batteries)
            {
                var tests = _context.Tests.Where(m => m.BatteryId == battery.Id);
                foreach (var test in tests)
                {
                    _context.Tests.Remove(test);
                }
                _context.Batteries.Remove(battery);
            }
            _context.Challenges.Remove(challengeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeModelExists(int id)
        {
            return _context.Challenges.Any(e => e.Id == id);
        }


        //Tests

        //GET
        [HttpGet]
        public IActionResult AddTest(int? id)
        {
            ViewData["ChallengeId"] = id;
            var challenge = _context.Challenges.FirstOrDefault(m => m.Id == id);
            if (challenge != null)
                ViewData["ChallengeName"] = challenge.Name;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTest([Bind("Input,ExpectedOutput,ChallengeId")] TestModel testModel)
        {
            if (ModelState.IsValid)
            {
                _context.Tests.Add(testModel);
                //challenge.Tests.Add(testModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = testModel.BatteryId;
            return View();
        }

        public IActionResult SelectLanguage(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult SelectLanguage(int? id, int? pos)
        {

            return RedirectToAction(nameof(Code));
        }

        [Authorize]
        public async Task<IActionResult> Code(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var challenge = await _context.Challenges.FirstOrDefaultAsync(m => m.Id == id);
            return View(challenge);
        }

        [HttpPost]
        public IActionResult Code(string code, string input, int? language)
        {
            ViewData["Message"] = "Your application description page.";
            string filename = null, type = null;
            switch (language)
            {
                case 0:
                    filename = "source.cpp";
                    type = "cpp";
                    break;
                case 1:
                    filename = "Solution.java";
                    type = "java";
                    break;
                case 2:
                    filename = "source.py";
                    type = "python";
                    break;
            }
            string url = "https://run.glot.io/languages/" + type + "/latest";
            string data;
            if (input != null)
            {
                data = "{\"stdin\": \"" + input + "\" , \"files\": [{\"name\": \"" + filename + "\", \"content\": \"" + code + "\"}]}";
            }
            else
            {
                data = "{\"files\": [{\"name\": \"" + filename + "\", \"content\": \"" + code + "\"}]}";
            }

            WebRequest myReq = WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentLength = data.Length;
            myReq.ContentType = "application/json; charset=UTF-8";

            string usernamePassword = "17a385d3-6c5e-4b76-8821-8e83b83352ff";

            UTF8Encoding enc = new UTF8Encoding();

            myReq.Headers.Add("Authorization: Token 17a385d3-6c5e-4b76-8821-8e83b83352ff");


            using (Stream ds = myReq.GetRequestStream())
            {
                ds.Write(enc.GetBytes(data), 0, data.Length);
            }

            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            string stdout = content.Split("stdout\":\"")[1].Split("\"")[0];
            string stderr = content.Split("stderr\":\"")[1].Split(",\"error")[0];
            string error = content.Split("error\":\"")[1].Split("\"}")[0];
            ViewData["stdout"] = stdout;
            ViewData["stderr"] = stderr;
            ViewData["error"] = error;
            return View();
        }


    }
}
