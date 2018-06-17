using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MTACodersLicence.Models;
using MTACodersLicence.Models.BatteryModels;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.GroupModels;
using MTACodersLicence.Models.ResultModels;
using MTACodersLicence.Models.SolutionModels;
using MTACodersLicence.Models.TestModels;
using MTACodersLicence.ViewModels;

namespace MTACodersLicence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<GroupModel> Groups { get; set; }

        public DbSet<GroupMemberModel> GroupMembers { get; set; }

        public DbSet<GroupContestModel> GroupContests { get; set; }

        public DbSet<ContestModel> Contests { get; set; }

        public DbSet<ChallengeModel> Challenges { get; set; }

        public DbSet<CodeTemplateModel> CodeTemplates { get; set; }

        public DbSet<TestModel> Tests { get; set; }

        public DbSet<BatteryModel> Batteries { get; set; }

        public DbSet<SolutionModel> Solutions { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<ResultModel> Results { get; set; }

        public DbSet<TestResultModel> TestResults { get; set; }

        public DbSet<CodingSessionModel> CodingSessions { get; set; }

        public DbSet<JoinGroupRequestModel> JoinGroupRequests{ get; set; }

        public DbSet<ProgrammingLanguageModel> ProgrammingLanguages { get; set; }

        public DbSet<RankingViewModel> Rankings { get; set; }

        public DbSet<ProfessorKey> ProfessorKeys { get; set; }

    }
}
