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
using MTACodersLicence.Models.ResultModels;
using Rotativa.AspNetCore;

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
                                    .Where(s => s.ChallengeId == challengeId)
                                    .OrderByDescending(s => s.Score);
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
                    case "gradeAsc":
                        solutions = solutions.OrderBy(s => s.Grade);
                        break;
                    case "gradeDesc":
                        solutions = solutions.OrderByDescending(s => s.Grade);
                        break;
                    default: break;
                }
            }
            var solution = solutions.FirstOrDefault();
            ViewData["challengeId"] = challengeId;
            if (solution != null)
            {
                ViewData["challengeName"] = solution.Challenge.Name;
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
                    .ThenInclude(s => s.Batteries)
                        .ThenInclude(s => s.Tests)
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

        private async Task RunTest(TestModel test, SolutionModel solution, ResultModel result)
        {
            TestResultModel testResult = new TestResultModel
            {
                TestId = test.Id,
                ResultId = result.Id
            };

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

            testResult.ResultedOutput = stdout;
            testResult.PointsGiven = stdout.Equals(test.ExpectedOutput) ? test.Points : 0;
            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();
        }

        private async Task VerifyIfResultExists(int? solutionId, int? batteryId)
        {
            var resultExistent = _context.Results
                                        .Where(s => s.BatteryId == batteryId && s.SolutionId == solutionId)
                                        .Include(s => s.TestResults);
            foreach (var resultModel in resultExistent)
            {
                _context.Results.Remove(resultModel);
            }
            await _context.SaveChangesAsync();
        }

        private async Task UpdateScoreAndGrade(SolutionModel solution)
        {
            float totalPointsGiven = 0;
            float totalPointsAvailable = 0;
            var results = _context.Results.Where(s => s.SolutionId == solution.Id)
                                           .Include(s => s.TestResults)
                                                .ThenInclude(s => s.Test);
            foreach (var result in results)
            {
                foreach (var testResult in result.TestResults)
                {
                    totalPointsGiven += testResult.PointsGiven;
                    totalPointsAvailable += testResult.Test.Points;
                }
            }
            if (results.Any())
            {
                solution.Verified = true;
            }
            solution.Score = totalPointsGiven;
            solution.Grade = (totalPointsGiven / totalPointsAvailable) * 10;
            await _context.SaveChangesAsync();
        }

        private async Task RunBattery(SolutionModel solution, BatteryModel battery)
        {
            // Verificam daca exista deja rezultate pentru bateria si solutia data si daca exista le suprascriem
            await VerifyIfResultExists(solution.Id, battery.Id);
            var result = new ResultModel
            {
                SolutionId = solution.Id,
                BatteryId = battery.Id
            };
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            foreach (var test in battery.Tests)
            {
                await RunTest(test, solution, result);
            }
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
            await RunBattery(solution, battery);
            await UpdateScoreAndGrade(solution);
            return RedirectToAction(nameof(Results), new { id, challengeId = battery.ChallengeId });

        }

        public IActionResult UserDetails(string userId)
        {
            var user = _context.ApplicationUser.FirstOrDefault(s => s.Id == userId);
            return View(user);
        }

        public IActionResult Results(int? id, int? challengeId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var results = _context.Results.Where(s => s.SolutionId == id)
                                            .Include(s => s.Solution)
                                            .Include(s => s.Battery)
                                            .Include(s => s.TestResults)
                                                .ThenInclude(s => s.Test);

            ViewData["challengeId"] = challengeId;
            return View(results);
        }

        public IActionResult PrintIndex(int? challengeId)
        {
            return new ViewAsPdf("Create");
        }

        public async Task<IActionResult> DeleteResult(int? resultId)
        {
            var result = _context.Results
                                .Include(s => s.TestResults)
                                .Include(s => s.Solution)
                                    .ThenInclude(s => s.Results)
                                .FirstOrDefault(s => s.Id == resultId);
            if (result == null)
            {
                return NotFound();
            }
            var solutionId = result.SolutionId;
            var challengeId = result.Solution.ChallengeId;
            var solution = result.Solution;
            if (result.Solution.Results.Count == 1)
            {
                result.Solution.Verified = false;
            }
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();
            UpdateScoreAndGrade(solution);
            return RedirectToAction(nameof(Results), new { id = solutionId, challengeId });
        }


        public async Task<IActionResult> VerifyAll(int? challengeId)
        {
            IList<SolutionModel> solutions = _context.Solutions
                                                    .Where(s => s.ChallengeId == challengeId)
                                                    .ToList();
            IList<BatteryModel> batteries = _context.Batteries
                                                    .Where(s => s.ChallengeId == challengeId)
                                                    .Include(s => s.Tests)
                                                    .ToList();
            foreach (var solution in solutions)
            {
                foreach (var battery in batteries)
                {
                      await RunBattery(solution, battery);
                }
                await UpdateScoreAndGrade(solution);
            }
            return RedirectToAction(nameof(Index), new { challengeId });
        }
    }
}
