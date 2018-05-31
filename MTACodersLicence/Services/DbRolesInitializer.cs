using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MTACodersLicence.Data;
using MTACodersLicence.Models;

namespace MTACodersLicence.Services
{
    public class DbRolesInitializer
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager)
        {
            // Look for any roles
            if (roleManager.Roles.Any())
            {
                return; // DB has been seeded
            }
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            await roleManager.CreateAsync(new IdentityRole("Profesor"));
            await roleManager.CreateAsync(new IdentityRole("Student"));
        }
    }
}
