using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTACodersLicence.Models.TestModels;

namespace MTACodersLicence.Models.ResultModels
{
    public class TestResultModel
    {
        public int Id { get; set; }

        public int TestId { get; set; }
        [ForeignKey(nameof(TestId))]
        public virtual TestModel Test { get; set; }

        public int ResultId { get; set; }
        [ForeignKey(nameof(ResultId))]
        public virtual ResultModel Result { get; set; }

        [Display(Name = "Resulted Output")]
        public string ResultedOutput { get; set; }

        [Display(Name = "Execution Time")]
        public decimal ExecutionTime { get; set; }

        [Display(Name = "Memory Used")]
        public float Memory { get; set; }
        
        [Display(Name = "Points Given")]
        public float PointsGiven { get; set; }
    }
}
