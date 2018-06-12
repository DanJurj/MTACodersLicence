using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;

namespace MTACodersLicence.Controllers
{
    public class CodeTemplateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodeTemplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CodeTemplate
        public async Task<IActionResult> Index(int challengeId, int? contestId)
        {
            var codeTemplates = await _context.CodeTemplates
                                            .Include(c => c.Challenge)
                                            .Include(c => c.ProgrammingLanguage)
                                            .Where(s => s.ChallengeId == challengeId)
                                            .ToListAsync();
            ViewData["ChallengeId"] = challengeId;
            ViewData["ContestId"] = contestId;
            return View(codeTemplates);
        }

        // GET: CodeTemplate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeTemplateModel = await _context.CodeTemplates
                .Include(c => c.Challenge)
                .Include(c => c.ProgrammingLanguage)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (codeTemplateModel == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = codeTemplateModel.Challenge.ContestId;
            return View(codeTemplateModel);
        }

        // GET: CodeTemplate/Create
        public IActionResult Create(int challengeId, int? contestId)
        {
            ViewData["ChallengeId"] = challengeId;
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
            return View();
        }

        // POST: CodeTemplate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,ProgrammingLanguageId,ChallengeId")] CodeTemplateModel codeTemplateModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(codeTemplateModel);
                await _context.SaveChangesAsync();
                var contestId = _context.CodeTemplates.Include(s => s.Challenge).FirstOrDefault(s => s.Id == codeTemplateModel.Id).Challenge.ContestId;
                return RedirectToAction(nameof(Index), new { codeTemplateModel.ChallengeId, contestId });
            }
            ViewData["ChallengeId"] = codeTemplateModel.ChallengeId;
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name", codeTemplateModel.ProgrammingLanguageId);
            return View(codeTemplateModel);
        }

        // GET: CodeTemplate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeTemplateModel = await _context.CodeTemplates.Include(s => s.Challenge).SingleOrDefaultAsync(m => m.Id == id);
            if (codeTemplateModel == null)
            {
                return NotFound();
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Name", codeTemplateModel.ChallengeId);
            ViewData["ContestId"] = codeTemplateModel.Challenge.ContestId;
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name", codeTemplateModel.ProgrammingLanguageId);
            return View(codeTemplateModel);
        }

        // POST: CodeTemplate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,ProgrammingLanguageId,ChallengeId")] CodeTemplateModel codeTemplateModel)
        {
            if (id != codeTemplateModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(codeTemplateModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodeTemplateModelExists(codeTemplateModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var contestId = _context.CodeTemplates.Include(s => s.Challenge).FirstOrDefault(s => s.Id == codeTemplateModel.Id).Challenge.ContestId;
                return RedirectToAction(nameof(Index), new { codeTemplateModel.ChallengeId, contestId });
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Name", codeTemplateModel.ChallengeId);
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name", codeTemplateModel.ProgrammingLanguageId);
            return View(codeTemplateModel);
        }

        // GET: CodeTemplate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codeTemplateModel = await _context.CodeTemplates
                .Include(s => s.Challenge)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (codeTemplateModel == null)
            {
                return NotFound();
            }
            var challengeId = codeTemplateModel.ChallengeId;
            var contestId = codeTemplateModel.Challenge.ContestId;
            _context.CodeTemplates.Remove(codeTemplateModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { challengeId, contestId });
        }

        private bool CodeTemplateModelExists(int id)
        {
            return _context.CodeTemplates.Any(e => e.Id == id);
        }
    }
}
