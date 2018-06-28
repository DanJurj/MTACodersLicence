using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.BatteryModels;
using MTACodersLicence.Models.ResultModels;
using MTACodersLicence.Models.SolutionModels;
using MTACodersLicence.Services;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class SolutionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolutionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Pagina in care profesorul poate vedea toate solutiile trimise
        // Solutiile sunt ordonate in functie de performanta, facand astfel un clasament
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Index(int? challengeId, string order)
        {
            var solutions = _context.Solutions
                                    .Include(s => s.Challenge)
                                        .ThenInclude(s => s.Batteries)
                                    .Include(s => s.Owner)
                                    .Include(s => s.ProgrammingLanguage)
                                    .Where(s => s.ChallengeId == challengeId)
                                    .OrderByDescending(s => s.Score)
                                        .ThenBy(s => s.ExecutionTime)
                                            .ThenBy(s => s.MemoryUsed);
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
            var challenge = _context.Challenges.Single(s => s.Id == challengeId);
            ViewData["challengeId"] = challengeId;
            ViewData["challengeName"] = challenge.Name;
            ViewData["ContestId"] = challenge.ContestId;
            return View(await solutions.ToListAsync());
        }

        // Detaliile unei Solutii
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var solutionModel = await _context.Solutions
                .Include(s => s.Challenge)
                    .ThenInclude(s => s.Batteries)
                        .ThenInclude(s => s.Tests)
                .Include(s => s.Owner)
                .Include(s => s.ProgrammingLanguage)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (solutionModel == null)
            {
                return NotFound();
            }
            return View(solutionModel);
        }

        // Stergerea unei solutii
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult Delete(int id)
        {
            var solution = _context.Solutions.SingleOrDefault(m => m.Id == id);
            if (solution == null)
            {
                return NotFound();
            }
            var challengeId = solution.ChallengeId;
            _context.Solutions.Remove(solution);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { challengeId });
        }

        // Verificam daca exista deja un rezultat pentru solutia si bateria respectiva, 
        // iar daca exista il stergem pentru a-l inlocui cu cel nou
        [Authorize(Roles = "Administrator,Profesor")]
        private void VerifyIfResultExists(int? solutionId, int? batteryId)
        {
            var resultExistent = _context.Results
                                        .Where(s => s.BatteryId == batteryId && s.SolutionId == solutionId)
                                        .Include(s => s.TestResults);
            foreach (var resultModel in resultExistent)
            {
                _context.Results.Remove(resultModel);
            }
            _context.SaveChanges();
        }

        // se verifica outputu-ul cu cel dorit si daca respecta cerintele legate de timpul de executie si memoria consumata
        private void CalculatePerformance(SolutionModel solution)
        {
            float totalPointsGiven = 0;
            float totalPointsAvailable = 0;
            var results = _context.Results.Where(s => s.SolutionId == solution.Id)
                                           .Include(s => s.TestResults)
                                                .ThenInclude(s => s.Test);
            float totalMemoy = 0;
            decimal totalTime = 0;
            if (results.Any())
            {
                solution.Verified = true;
            }
            else
            {
                solution.Verified = false;
                solution.Score = 0;
                solution.Grade = 0;
                solution.ExecutionTime = 0;
                solution.MemoryUsed = 0;
                _context.SaveChanges();
                return;
            }
            foreach (var result in results)
            {
                foreach (var testResult in result.TestResults)
                {
                    totalPointsGiven += testResult.PointsGiven;
                    totalPointsAvailable += testResult.Test.Points;
                    totalTime += testResult.ExecutionTime;
                    totalMemoy += testResult.Memory;
                }
            }
            solution.Score = totalPointsGiven;
            solution.Grade = (totalPointsGiven / totalPointsAvailable) * 10;
            solution.ExecutionTime = totalTime;
            solution.MemoryUsed = totalMemoy;
            _context.SaveChanges();
        }

        // Verificarea unei solutii pe o baterie de teste
        [Authorize(Roles = "Administrator,Profesor")]
        private async Task RunBattery(SolutionModel solution, BatteryModel battery)
        {
            // Verificam daca exista deja rezultate pentru bateria si solutia data si daca exista le suprascriem
            VerifyIfResultExists(solution.Id, battery.Id);
            var result = new ResultModel
            {
                SolutionId = solution.Id,
                BatteryId = battery.Id
            };
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            foreach (var test in battery.Tests)
            {
                var testResult = CodeRunner.RunCode(solution.Code, test, solution.ProgrammingLanguage.LanguageCode);
                testResult.ResultId = result.Id;
                testResult.TestId = test.Id;
                var expectedOutput = test.ExpectedOutput.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\n", "").Replace(" ", "");
                var resultedOutput = testResult.ResultedOutput.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\t", "").Replace("\n", "").Replace(" ", "");
                var pointsGiven = expectedOutput.Equals(resultedOutput) ? test.Points : 0;
                testResult.PointsGiven = pointsGiven;
                if (testResult.ExecutionTime <= solution.Challenge.ExecutionTimeLimit && testResult.Memory <= solution.Challenge.MemoryLimit)
                {
                    // inserare in tabelul de rezultate ale testelor
                    _context.TestResults.Add(testResult);
                    _context.SaveChanges();
                }
            }
        }

        // verificarea unei solutii pe o baterie de teste la alegere
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> Run(int? id, int? batteryId)
        {
            if (id == null || batteryId == null)
            {
                return NotFound();
            }

            var solution = await _context.Solutions
                .Include(s => s.ProgrammingLanguage)
                .FirstOrDefaultAsync(m => m.Id == id);
            var battery = await _context.Batteries
                                        .Include(t => t.Tests)
                                        .Include(s => s.Challenge)
                                        .FirstOrDefaultAsync(m => m.Id == batteryId);
            await RunBattery(solution, battery);
            CalculatePerformance(solution);
            return RedirectToAction(nameof(Results), new { id, challengeId = battery.ChallengeId });

        }

        // Detaliile unui din utilizatorii care au trimis solutiile
        [Authorize(Roles = "Administrator,Profesor")]
        public IActionResult UserDetails(string userId)
        {
            var user = _context.ApplicationUser.FirstOrDefault(s => s.Id == userId);
            return View(user);
        }

        /// <summary>
        ///  Rezultatele solutiei pe bateriile de teste
        /// </summary>
        /// <param name="id">id-ul solutiei</param>
        /// <param name="challengeId">id-ul problemei</param>
        [Authorize(Roles = "Administrator,Profesor")]
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

        // Stergerea unui rezultat
        [Authorize(Roles = "Administrator,Profesor")]
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
            CalculatePerformance(solution);
            return RedirectToAction(nameof(Results), new { id = solutionId, challengeId });
        }

        // Actiune de reverificare a tuturor solutiilor pe toate bateriile de teste
        [Authorize(Roles = "Administrator,Profesor")]
        public async Task<IActionResult> VerifyAll(int? challengeId)
        {
            IList<SolutionModel> solutions = _context.Solutions
                                                    .Include(s => s.ProgrammingLanguage)
                                                    .Where(s => s.ChallengeId == challengeId)
                                                    .ToList();
            IList<BatteryModel> batteries = _context.Batteries
                                                    .Where(s => s.ChallengeId == challengeId)
                                                    .Include(s => s.Tests)
                                                    .Include(s => s.Challenge)
                                                    .ToList();
            foreach (var solution in solutions)
            {
                foreach (var battery in batteries)
                {
                    await RunBattery(solution, battery);
                }
                CalculatePerformance(solution);
            }
            return RedirectToAction(nameof(Index), new { challengeId });
        }

        // Actiune apelata in momentul in care se face un submit Code pentru a verica codul 
        // si a-l putea redirectiona pe utilizator la clasamentul problemei
        public async Task<IActionResult> VerifySubmit(int id)
        {
            var solution = _context.Solutions
                .Include(s => s.ProgrammingLanguage)
                .Include(s => s.Challenge)
                .FirstOrDefault(s => s.Id == id);
            var batteries = _context.Batteries
                .Where(s => s.ChallengeId == solution.ChallengeId)
                .Include(s => s.Tests)
                .ToList();
            foreach (var battery in batteries)
            {
                var result = new ResultModel
                {
                    SolutionId = solution.Id,
                    BatteryId = battery.Id
                };
                _context.Results.Add(result);
                _context.SaveChanges();
                foreach (var test in battery.Tests)
                {
                    var testResult = CodeRunner.RunCode(solution.Code, test, solution.ProgrammingLanguage.LanguageCode);
                    testResult.ResultId = result.Id;
                    testResult.TestId = test.Id;
                    // eliminam caracterele de editare a output-ului
                    var expectedOutput = test.ExpectedOutput.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\n", "").Replace(" ", "");
                    var resultedOutput = testResult.ResultedOutput.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\t", "").Replace("\n", "").Replace(" ", "");
                    var pointsGiven = expectedOutput.Equals(resultedOutput) ? test.Points : 0;
                    // primeste punctele doar daca respecta cerintele de performanta
                    if (pointsGiven > 0)
                        if (testResult.ExecutionTime <= solution.Challenge.ExecutionTimeLimit && testResult.Memory <= solution.Challenge.MemoryLimit)
                        {
                            testResult.PointsGiven = pointsGiven;
                        }

                    // inserare in tabelul de rezultate ale testelor
                    _context.TestResults.Add(testResult);
                    await _context.SaveChangesAsync();
                }
            }
            CalculatePerformance(solution);
            //await UpdateScoreAndGrade(solution);
            return RedirectToAction("Ranking", "Challenge", new { id = solution.ChallengeId });
        }
    }
}
