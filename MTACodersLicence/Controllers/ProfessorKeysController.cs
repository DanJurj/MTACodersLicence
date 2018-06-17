using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;

namespace MTACodersLicence.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProfessorKeysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorKeysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista tututor key-urilor disponibile
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProfessorKeys.ToListAsync());
        }

        // GET: ProfessorKeys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProfessorKeys/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Key,NumberOfAccountsAvailable")] ProfessorKey professorKey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(professorKey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(professorKey);
        }

        // GET: ProfessorKeys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professorKey = await _context.ProfessorKeys.SingleOrDefaultAsync(m => m.Id == id);
            if (professorKey == null)
            {
                return NotFound();
            }
            return View(professorKey);
        }

        // POST: ProfessorKeys/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Key,NumberOfAccountsAvailable")] ProfessorKey professorKey)
        {
            if (id != professorKey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professorKey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorKeyExists(professorKey.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(professorKey);
        }

        // GET: ProfessorKeys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professorKey = await _context.ProfessorKeys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (professorKey == null)
            {
                return NotFound();
            }
            _context.ProfessorKeys.Remove(professorKey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfessorKeyExists(int id)
        {
            return _context.ProfessorKeys.Any(e => e.Id == id);
        }
    }
}
