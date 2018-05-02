using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MTACodersLicence.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName;

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName;

        public ICollection<GroupModel> Groups { get; set; }
        public ICollection<ChallengeModel> Challenges { get; set; }
    }
}
