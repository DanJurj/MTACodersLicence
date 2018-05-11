using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GroupModel> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<MTACodersLicence.Models.GroupItem> GroupItems { get; set; }

        public DbSet<MTACodersLicence.Models.ChallengeModel> Challenges { get; set; }

        public DbSet<MTACodersLicence.Models.TestModel> Tests { get; set; }

        public DbSet<MTACodersLicence.Models.ChallengeModels.BatteryModel> Batteries { get; set; }
    }
}
