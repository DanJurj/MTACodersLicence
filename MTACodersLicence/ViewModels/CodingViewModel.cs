using System.Collections.Generic;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Services;

namespace MTACodersLicence.ViewModels
{
    public class CodingViewModel
    {
        public ChallengeModel Challenge { get; set; }
        public CodingSessionModel CodingSession { get; set; }
        public IList<ProgrammingLanguageModel> ProgramingLanguages { get; set; }
        public int RemainingTime { get; set; } //minutes
        public bool HasRemainingTime { get; set; }
       // public CodeRunnerResult CodeResult { get; set; }
        public bool HasGrade { get; set; }
        public float Grade { get; set; }
        public string Language { get; set; }
    }
}