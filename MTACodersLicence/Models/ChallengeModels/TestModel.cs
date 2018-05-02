using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Input { get; set; }
        [Display(Name = "Expected Output")]
        public string ExpectedOutput { get; set; }

        public int? ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }
    }
}
