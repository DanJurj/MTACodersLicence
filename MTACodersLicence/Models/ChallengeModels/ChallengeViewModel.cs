using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models.ChallengeModels
{
    public class ChallengeViewModel
    {
        public ChallengeModel Challenge { get; set; }
        public CodingSessionModel CodingSession { get; set; }
        public bool HasPreviousSave { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan RemainingTime { get; set; }
        public bool HasRemainingTime { get; set; }
    }
}
