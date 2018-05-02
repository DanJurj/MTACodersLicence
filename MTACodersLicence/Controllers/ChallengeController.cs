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
        public async Task<IActionResult> Index(string searchString)
        {

            var applicationDbContext = _context.Challenges.Include(c => c.Tests).
                                                                Where(c => c.ApplicationUserId == _userManager.GetUserId(User));
            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Name.Contains(searchString));
            }
            return View(await applicationDbContext.ToListAsync());
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
                .Include(c => c.Tests)
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Challenge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Desciption,Time,Hint")] ChallengeModel challengeModel)
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", challengeModel.ApplicationUserId);
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", challengeModel.ApplicationUserId);
            return View(challengeModel);
        }

        // POST: Challenge/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Desciption,Time,Hint,ApplicationUserId")] ChallengeModel challengeModel)
        {
            if (id != challengeModel.Id)
            {
                return NotFound();
            }

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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", challengeModel.ApplicationUserId);
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
            ViewData["ChallengeId"] = testModel.ChallengeId;
            return View();
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
        public IActionResult Code(string code, string input)
        {
            ViewData["Message"] = "Your application description page.";

            string url = "https://run.glot.io/languages/cpp/latest";
            string data = null;
            if (input != null)
            {
                data = "{\"stdin\": \"" + input + "\" , \"files\": [{\"name\": \"source.cpp\", \"content\": \"" + code + "\"}]}";
            }
            else
            {
                data = "{\"files\": [{\"name\": \"source.cpp\", \"content\": \"" + code + "\"}]}";
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
            //Response.Write(content);
            string[] response = content.Split("stdout\":\"");
            //string stdout = response[0].Split(":")[1];
            //string stderr = response[1].Split(":")[1].Split("\"")[1];
            //string error = response[2];
            //string stdout2 = stdout.Split("\"")[1];
           // ResponseModel response = JsonConvert.DeserializeObject<ResponseModel>(content);
            //ViewData["stdout"] = stdout2;
            //ViewData["stderr"] = stderr;
            //ViewData["error"] = error;
            ViewData["result"] = content;
            return View();
        }
    }
}
