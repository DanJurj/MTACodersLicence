using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Controllers
{
    public class SolutionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolutionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Solution
        public async Task<IActionResult> Index(int? challengeId, string order)
        {
            var solutions = _context.Solutions
                                    .Include(s => s.Challenge)
                                        .ThenInclude(s => s.Batteries)
                                    .Include(s => s.Owner)
                                    .Where(s => s.ChallengeId == challengeId);
            if (order != null)
            {
                switch (order)
                {
                    case "scoreAsc":
                        solutions = solutions.OrderBy(s => s.Score);
                        break;
                    case "scoreDesc":
                        solutions = solutions.OrderByDescending(s => s.Score);
                        break;
                    case "dateAsc":
                        solutions = solutions.OrderBy(s => s.ReceiveDateTime);
                        break;
                    case "dateDesc":
                        solutions = solutions.OrderByDescending(s => s.ReceiveDateTime);
                        break;
                    case "durationAsc":
                        solutions = solutions.OrderBy(s => s.Duration);
                        break;
                    case "durationDesc":
                        solutions = solutions.OrderByDescending(s => s.Duration);
                        break;
                    case "ownerAsc":
                        solutions = solutions.OrderBy(s => s.Owner.UserName);
                        break;
                    case "ownerDesc":
                        solutions = solutions.OrderByDescending(s => s.Owner.UserName);
                        break;
                }
            }
            return View(await solutions.ToListAsync());
        }

        // GET: Solution/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solutionModel = await _context.Solutions
                .Include(s => s.Challenge)
                .Include(s => s.Owner)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (solutionModel == null)
            {
                return NotFound();
            }

            return View(solutionModel);
        }

        // GET: Solution/Create
        public IActionResult Create()
        {
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id");
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Solution/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Verified,Score,ReceiveDateTime,TimeSpent,ChallengeId,ApplicationUserId")] SolutionModel solutionModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solutionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", solutionModel.ChallengeId);
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", solutionModel.ApplicationUserId);
            return View(solutionModel);
        }

        // GET: Solution/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solutionModel = await _context.Solutions.SingleOrDefaultAsync(m => m.Id == id);
            if (solutionModel == null)
            {
                return NotFound();
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", solutionModel.ChallengeId);
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", solutionModel.ApplicationUserId);
            return View(solutionModel);
        }

        // POST: Solution/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Verified,Score,ReceiveDateTime,TimeSpent,ChallengeId,ApplicationUserId")] SolutionModel solutionModel)
        {
            if (id != solutionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solutionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolutionModelExists(solutionModel.Id))
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
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", solutionModel.ChallengeId);
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", solutionModel.ApplicationUserId);
            return View(solutionModel);
        }

        // GET: Solution/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solutionModel = await _context.Solutions.SingleOrDefaultAsync(m => m.Id == id);
            var challengeId = solutionModel.ChallengeId;
            _context.Solutions.Remove(solutionModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { challengeId });
        }

        private bool SolutionModelExists(int id)
        {
            return _context.Solutions.Any(e => e.Id == id);
        }

        private int RunTest(TestModel test, SolutionModel solution)
        {
            var type = "cpp";
            var filename = "source.cpp";
            string url = "https://run.glot.io/languages/" + type + "/latest";
            string data = "{\"stdin\": \"" + test.Input + "\" , \"files\": [{\"name\": \"" + filename + "\", \"content\": \"" + solution.Code + "\"}]}";
           

            WebRequest myReq = WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.ContentLength = data.Length;
            myReq.ContentType = "application/json; charset=UTF-8";
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
            return 1;
        }

        private int RunTests(BatteryModel battery, SolutionModel solution)
        {
            int points = 0;
            foreach (var test in battery.Tests)
            {
                points += RunTest(test, solution);
            }
            return points;
        }

        public async Task<IActionResult> Run(int? id, int? batteryId)
        {
            if (id == null || batteryId == null)
            {
                return NotFound();
            }

            var solution = await _context.Solutions.FirstOrDefaultAsync(m => m.Id == id);
            var battery = await _context.Batteries
                                        .Include(t => t.Tests)
                                        .FirstOrDefaultAsync(m => m.Id == batteryId);
            int points = RunTests(battery, solution);
            solution.Verified = true;
            solution.Score = points;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {challengeId = battery.ChallengeId});
        }
    }
}
