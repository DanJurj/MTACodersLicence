using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.BatteryModels;

namespace MTACodersLicence.Controllers
{
    [Authorize(Roles = "Administrator,Profesor")]
    public class BatteryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BatteryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <param name="challengeId">id-ul problemei pentru care dorim lista de baterii de teste disponibile</param>
        /// <returns>Lista de baterii de teste pentru problema cu id-ul: challengeId</returns>
        public async Task<IActionResult> Index(int? challengeId)
        {
            if (challengeId == null)
            {
                return NotFound();
            }
            var batteries = _context.Batteries.
                                    Include(b => b.Challenge).
                                    Include(b => b.Tests).
                                    Where(c => c.ChallengeId == challengeId);
            var challenge = _context.Challenges.FirstOrDefault(c => c.Id == challengeId);
            if (challenge != null)
            {
                ViewData["challengeName"] = challenge.Name;
                ViewData["challengeId"] = challengeId;
                ViewData["ContestId"] = challenge.ContestId;
            }
            return View(await batteries.ToListAsync());
        }

        /// <param name="id">id-ul bateriei de teste</param>
        /// <returns>Detaliile bateriei de teste cu id-ul = id</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryModel = await _context.Batteries
                .Include(b => b.Challenge)
                .Include(b => b.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (batteryModel == null)
            {
                return NotFound();
            }

            return View(batteryModel);
        }

        /// <summary>
        /// Actiunea aferenta unui request de tip GET pentru crearea unei baterii de teste
        /// </summary>
        /// <param name="challengeId">id-ul problemei pentru care se doreste crearea bateriei de teste</param>
        /// <returns>View-ul pentru crearea unei noi baterii de teste</returns>
        public IActionResult Create(int? challengeId)
        {
            ViewData["ChallengeId"] = challengeId;
            return View();
        }

        /// <summary>
        /// POST-ul cu datele de crearea a unei noi baterii de teste
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsPublic,ChallengeId")] BatteryModel batteryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(batteryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {challengeId = batteryModel.ChallengeId});
            }
            return View(batteryModel);
        }

        /// <param name="id">id-ul bateriei</param>
        /// <returns>Pagina de editare a unei baterii de teste</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batteryModel = await _context.Batteries
                                            .Include(m => m.Tests)
                                            .SingleOrDefaultAsync(m => m.Id == id);
            if (batteryModel == null)
            {
                return NotFound();
            }
            return View(batteryModel);
        }

        /// <summary>
        /// POST-ul cu datele modificate ale bateriei de teste
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsPublic,ChallengeId")] BatteryModel batteryModel)
        {
            if (id != batteryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(batteryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatteryModelExists(batteryModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new {challengeId = batteryModel.ChallengeId});
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Id", batteryModel.ChallengeId);
            return View(batteryModel);
        }

        /// <summary>
        /// Cerere de tip GET pentru stergerea unei baterii de teste
        /// </summary>
        /// <param name="id">id-ul bateriei ce se doreste a fi stearsa</param>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var batteryModel = await _context.Batteries
                .Include(b => b.Challenge)
                .Include(b => b.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (batteryModel == null)
                return NotFound();

            return View(batteryModel);
        }

        /// <summary>
        /// POST-ul cu confirmarea cereri de stergere
        /// </summary>
        /// <param name="id">id-ul bateriei ce se doreste a fi stearsa</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tests = _context.Tests.Where(b => b.BatteryId == id);
            var batteryModel = await _context.Batteries
                                            .Include(m =>m.Tests)
                                            .SingleOrDefaultAsync(m => m.Id == id);
            var challengeId = batteryModel.ChallengeId;
            foreach (var test in tests)
            {
                _context.Tests.Remove(test);
            }
            _context.Batteries.Remove(batteryModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { challengeId });
        }

        private bool BatteryModelExists(int id)
        {
            return _context.Batteries.Any(e => e.Id == id);
        }
    }
}
