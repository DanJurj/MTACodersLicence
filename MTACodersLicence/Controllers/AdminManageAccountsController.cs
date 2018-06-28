using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminManageAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminManageAccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        ///  Afiseaza toate conturile din baza de date impreuna cu rolurile acestora printr-un ViewModel
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.ApplicationUser.ToListAsync();
            var adminManageViewModelList = new List<AdminManageAccountViewModel>();
            foreach (var account in accounts)
            {
                var roles = await _userManager.GetRolesAsync(account);
                adminManageViewModelList.Add(new AdminManageAccountViewModel()
                {
                    Account = account,
                    Roles = roles
                });
            }
            var codingSessionsCount = _context.CodingSessions.Count();
            ViewData["codingSessions"] = codingSessionsCount;
            return View(adminManageViewModelList);
        }

        /// <summary>
        /// Cerere GET de Editare a unui cont
        /// </summary>
        /// <param name="id">id-ul contului</param> 
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        /// <summary>
        /// Cerere POST de editare a contului
        /// </summary>
        /// <param name="id">id-ul contului</param>
        /// <param name="applicationUser">modelul format din totalitatea parametrilor trimisi</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _context.ApplicationUser.SingleOrDefaultAsync(s => s.Id == applicationUser.Id);
                    account.FirstName = applicationUser.FirstName;
                    account.LastName = applicationUser.LastName;
                    account.UserName = applicationUser.UserName;
                    account.NormalizedUserName = applicationUser.NormalizedUserName;
                    account.Email = applicationUser.Email;
                    account.NormalizedEmail = applicationUser.NormalizedEmail;
                    account.EmailConfirmed = applicationUser.EmailConfirmed;
                    account.PhoneNumber = applicationUser.PhoneNumber;
                    _context.ApplicationUser.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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
            return View(applicationUser);
        }

        /// <summary>
        /// Cerere de tip GET de stergere a unui cont
        /// </summary>
        /// <param name="id">id-ul contului ce urmeaza a fi sters</param>
        /// <returns>view-ul de confirmare a stergerii</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        /// <summary>
        /// Cerere de tip POST in urma confirmarii de stergere a contului
        /// </summary>
        /// <param name="id">id-ul contului ce urmeaza a fi sters</param>
        /// <returns>pagina de Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser
                .Include(s => s.Contests)
                .Include(s => s.Challenges)
                    .ThenInclude(s => s.Solutions)
                .Include(s=>s.Challenges).ThenInclude(s=>s.Batteries).ThenInclude(s=>s.Tests)
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// Verifica daca utilizatorul cu id-ul id exista in baza de date
        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }

        /// <summary>
        /// Folosita pentru a sterge sesiunile de cod existente
        /// </summary>
        /// <returns>Pagina de Index</returns>
        public async Task<IActionResult> ClearCodingSessions()
        {
            var codingSessions = await _context.CodingSessions.ToListAsync();
            foreach (var codingSession in codingSessions)
            {
                _context.CodingSessions.Remove(codingSession);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
