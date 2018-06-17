using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class PracticeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PracticeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Pagina cu problemele disponibile pentru antrenament
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Challenges
                                            .Where(s => s.AvailableForPractice);
            return View(await applicationDbContext.ToListAsync());
        }

        // Pagina de rezolvare a problemei
        public IActionResult Start(int challengeId)
        {
            var challenge = _context.Challenges.FirstOrDefault(s => s.Id == challengeId);
            if (challenge == null)
            {
                return NotFound();
            }
            var programmingLanguages = _context.ProgrammingLanguages.Where(s => s.Available).ToList();
            var codeTemplates = _context.CodeTemplates.Where(s => s.ChallengeId == challengeId);
            foreach (var codeTemplate in codeTemplates)
            {
                foreach (var language in programmingLanguages)
                {
                    if (codeTemplate.ProgrammingLanguageId == language.Id)
                    {
                        language.CodeTemplate = codeTemplate.Code;
                    }
                }
            }
            var programmingLanguage = _context.ProgrammingLanguages.FirstOrDefault(s => s.Available);
            var programmingLanguagesSelectList = new SelectList(programmingLanguages, "LanguageCode", "Name", programmingLanguage.LanguageCode);
            ViewData["ProgrammingLanguages"] = programmingLanguagesSelectList;
            var practice = new PracticeViewModel();
            practice.Challenge = challenge;
            practice.ProgramingLanguages = programmingLanguages;
            return View(practice);
        }
    }
}
