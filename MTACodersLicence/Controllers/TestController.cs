using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models.TestModels;

namespace MTACodersLicence.Controllers
{
    [Authorize(Roles = "Administrator,Profesor")]
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <param name="batteryId">id-ul bateriei de teste pentru care se doreste afisarea testelor</param>
        /// <returns>Pagina cu testele aferente bateriei cu id-ul dat</returns>
        public async Task<IActionResult> Index(int? batteryId)
        {
            var tests = _context.Tests.Include(t => t.Battery).Where(c => c.BatteryId == batteryId);
            var battery = await _context.Batteries.FirstOrDefaultAsync(m => m.Id == batteryId);
            ViewData["batteryId"] = batteryId;
            ViewData["batteryName"] = battery.Name;
            ViewData["challengeId"] = battery.ChallengeId;
            return View(await tests.ToListAsync());
        }

        // GET: Test/Create
        public async Task<IActionResult> Create(int? batteryId)
        {
            var battery = await _context.Batteries.FirstOrDefaultAsync(m => m.Id == batteryId);
            ViewData["batteryId"] = batteryId;
            ViewData["batteryName"] = battery.Name;
            return View();
        }

        // POST: Test/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Input,ExpectedOutput,Points,BatteryId")] TestModel testModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {batteryId = testModel.BatteryId});
            }
            return View(testModel);
        }

        // GET: Test/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModel = await _context.Tests.SingleOrDefaultAsync(m => m.Id == id);
            if (testModel == null)
            {
                return NotFound();
            }
            ViewData["BatteryId"] = new SelectList(_context.Batteries, "Id", "Id", testModel.BatteryId);
            return View(testModel);
        }

        // POST: Test/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Input,ExpectedOutput,Points,BatteryId")] TestModel testModel)
        {
            if (id != testModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestModelExists(testModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new {batteryId = testModel.BatteryId});
            }
            ViewData["BatteryId"] = new SelectList(_context.Batteries, "Id", "Id", testModel.BatteryId);
            return View(testModel);
        }

        // GET: Test/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testModel = await _context.Tests
                .Include(t => t.Battery)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (testModel == null)
            {
                return NotFound();
            }

            return View(testModel);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testModel = await _context.Tests.SingleOrDefaultAsync(m => m.Id == id);
            _context.Tests.Remove(testModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {batteryId = testModel.BatteryId});
        }

        private bool TestModelExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}
