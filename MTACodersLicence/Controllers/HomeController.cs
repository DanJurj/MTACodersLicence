using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using System.Security.Claims;
using MTACodersLicence.Services;
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext _ctx;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
           /* var prog = _ctx.ProgrammingLanguages.FirstOrDefault();
            string code =
                "#include <stdio.h>\n int main(void) {\n  char name[10];\n  scanf(\"%s\", name);  return 0;\n";

            CodeRunner2.RunCode(
                "#include <stdio.h>\\n int main(void) {\\n  char name[10];\\n  scanf(\\\"%s\\\", name);  return 0;\\n",
                "Judge0", prog);*/
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
