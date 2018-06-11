using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models
{
    public class CodeTemplateModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public int ProgrammingLanguageId { get; set; }
        public ProgrammingLanguageModel ProgrammingLanguage { get; set; }

        public int ChallengeId { get; set; }
        public ChallengeModel Challenge { get; set; }
    }
}