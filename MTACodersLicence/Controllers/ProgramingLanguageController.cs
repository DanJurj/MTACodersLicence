﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;

namespace MTACodersLicence.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProgramingLanguageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramingLanguageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProgramingLanguage
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProgrammingLanguages.ToListAsync());
        }

        // GET: ProgramingLanguage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programingLanguageModel = await _context.ProgrammingLanguages.SingleOrDefaultAsync(m => m.Id == id);
            if (programingLanguageModel == null)
            {
                return NotFound();
            }
            return View(programingLanguageModel);
        }

        // POST: ProgramingLanguage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Filename,Type,Available,CodeTemplate")] ProgrammingLanguageModel programingLanguageModel)
        {
            if (id != programingLanguageModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programingLanguageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramingLanguageModelExists(programingLanguageModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(programingLanguageModel);
        }

        private bool ProgramingLanguageModelExists(int id)
        {
            return _context.ProgrammingLanguages.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            var programmingLanguage = await _context.ProgrammingLanguages.SingleOrDefaultAsync(s => s.Id == id);
            programmingLanguage.Available = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activate(int id)
        {
            var programmingLanguage = await _context.ProgrammingLanguages.SingleOrDefaultAsync(s => s.Id == id);
            programmingLanguage.Available = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
