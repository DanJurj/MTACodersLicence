using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTACodersLicence.Models.ChallengeModels
{
    public class CodingSessionModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public int ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }

        public string Code { get; set; }
        public bool HasPreviousSave { get; set; }

        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguageModel ProgrammingLanguage { get; set; }
    }
}
