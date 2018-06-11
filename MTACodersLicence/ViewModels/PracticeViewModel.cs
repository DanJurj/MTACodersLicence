using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.ViewModels
{
    public class PracticeViewModel
    {
        public ChallengeModel Challenge { get; set; }
        public IList<ProgrammingLanguageModel> ProgramingLanguages { get; set; }
        public string Language { get; set; }
    }
}
