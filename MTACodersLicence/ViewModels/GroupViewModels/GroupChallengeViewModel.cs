using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.ViewModels.GroupViewModels
{
    public class GroupChallengeViewModel
    {
        public ICollection<ChallengeModel> AvailableChallenges { get; set; }
        public ICollection<GroupChallengeModel> AssignedChallenges { get; set; }
    }
}
