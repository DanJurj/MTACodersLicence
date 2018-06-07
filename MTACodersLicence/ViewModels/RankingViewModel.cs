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
    }
}
