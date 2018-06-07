using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.ViewModels
{
    public class EditContestViewModel
    {
        public int Id { get; set; }
        public ContestModel Contest { get; set; }
        public IList<ChallengeModel> Challenges { get; set; }
    }
}
