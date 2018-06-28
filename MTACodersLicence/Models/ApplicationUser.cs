using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName => LastName + " " + FirstName;

        public ICollection<GroupMemberModel> Groups { get; set; }
        public ICollection<ChallengeModel> Challenges { get; set; }
        public ICollection<ContestModel> Contests { get; set; }
    }
}
