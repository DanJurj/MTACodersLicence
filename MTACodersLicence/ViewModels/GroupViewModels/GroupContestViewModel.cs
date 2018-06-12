using System.Collections.Generic;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.ViewModels.GroupViewModels
{
    public class GroupContestViewModel
    {
        public ICollection<ContestModel> AvailableContests { get; set; }
        public ICollection<GroupContestModel> AssignedContests { get; set; }
    }
}
