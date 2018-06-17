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

        /// <summary>
        /// Returneaza lista de sabloane de cod pentru o problema din cadrul unui concurs
        /// </summary>
        /// <param name="challengeId">id-ul problemei</param>
        /// <param name="contestId">id-ul concursului</param>
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

        /// <param name="id">id-ul sablonului de cod</param>
        /// <returns>Pagina de detalii pentru sablonul de cod cu id-ul dat</returns>
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

        /// <param name="challengeId">id-ul problemei</param>
        /// <param name="contestId">id-ul concursului</param>
        /// <returns>Pagina de creare a unui nou sablon pentru o problema din cadul unui concurs</returns>
        public IActionResult Create(int challengeId, int? contestId)
        {
            ViewData["ChallengeId"] = challengeId;
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name");
            return View();
        }

        /// <summary>
        /// Actiunea apelata in urma submit-ului cu datele de creare a unui nou sablon
        /// </summary>
        /// <param name="codeTemplateModel">modelul care inglobeaza toate datele din form</param>
        /// <returns>Pagina Index in caz de succes sau pagina Create in cazul unui esec</returns>
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

        /// <param name="id">id-ul sablonului de code</param>
        /// <returns>Pagina de editare a sablonui de cod cu id-ul dat</returns>
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

        /// <summary>
        /// Actiunea apelata in momentul unui submit din pagina de Edit
        /// </summary>
        /// <param name="id">id-ul sablonului de cod</param>
        /// <param name="codeTemplateModel">modelul cu toate datele modificate</param>
        /// <returns>Index in caz de succes sau Edit in caz de esec</returns>
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
                    throw;
                }
                var contestId = _context.CodeTemplates.Include(s => s.Challenge).FirstOrDefault(s => s.Id == codeTemplateModel.Id).Challenge.ContestId;
                return RedirectToAction(nameof(Index), new { codeTemplateModel.ChallengeId, contestId });
            }
            ViewData["ChallengeId"] = new SelectList(_context.Challenges, "Id", "Name", codeTemplateModel.ChallengeId);
            ViewData["ProgrammingLanguageId"] = new SelectList(_context.ProgrammingLanguages, "Id", "Name", codeTemplateModel.ProgrammingLanguageId);
            return View(codeTemplateModel);
        }

        /// <summary>
        /// Actiune apelata pentru stergerea unui sablon
        /// </summary>
        /// <param name="id">id-ul sablonului</param>
        /// <returns>Index in caz de succes sau NotFound in caz de esec</returns>
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
