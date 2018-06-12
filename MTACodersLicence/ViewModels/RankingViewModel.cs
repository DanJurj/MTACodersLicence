using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.ViewModels
{
    public class RankingViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Sent by")]
        public string SentBy { get; set; }
        public float Score { get; set; }
        public float Grade { get; set; }
        [Display(Name = "Execution time")]
        public decimal TotalExecutionTime { get; set; }
        [Display(Name = "Memory Used")]
        public float TotalMemoryUsed { get; set; }
        public string Language { get; set; }
    }
}
