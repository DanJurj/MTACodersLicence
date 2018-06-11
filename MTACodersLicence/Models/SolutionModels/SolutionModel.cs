using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.ResultModels;

namespace MTACodersLicence.Models.SolutionModels
{
    public class SolutionModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Verified { get; set; }
        public float Score { get; set; }
        public float Grade { get; set; }
        public float Time { get; set; }
        public float Memory { get; set; }
        public string Language { get; set; }
        [Display(Name = "Received Time")]
        public DateTime ReceiveDateTime { get; set; }

        public TimeSpan Duration { get; set; }

        public int ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }

        public ICollection<ResultModel> Results { get; set; }

        public int ProgrammingLanguageId { get; set; }
        [Display(Name = "Language")]
        public ProgrammingLanguageModel ProgrammingLanguage { get; set; }
    }
}
