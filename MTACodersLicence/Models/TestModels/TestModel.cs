using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTACodersLicence.Models.BatteryModels;

namespace MTACodersLicence.Models.TestModels
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Input { get; set; }
        [Display(Name = "Expected Output")]
        public string ExpectedOutput { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
        public float Points { get; set; }

        public int? BatteryId { get; set; }
        [ForeignKey(nameof(BatteryId))]
        public virtual BatteryModel Battery { get; set; }
    }
}
