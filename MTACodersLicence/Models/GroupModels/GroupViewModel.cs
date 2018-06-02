using System.ComponentModel.DataAnnotations;

namespace MTACodersLicence.Models.GroupModels
{
    public class GroupViewModel : GroupModel
    {
        [Display(Name = "Members")]
        public int NumberOfMembers { get; set; }

        [Display(Name = "Challenges")]
        public int NumberOfChallenges { get; set; }

        [Display(Name = "Active Challenges")]
        public int ActiveChallenges { get; set; }
    }
}
