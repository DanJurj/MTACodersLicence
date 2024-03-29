﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTACodersLicence.Models.BatteryModels;
using MTACodersLicence.Models.GroupModels;
using MTACodersLicence.Models.SolutionModels;

namespace MTACodersLicence.Models.ChallengeModels
{
    public class ChallengeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public string Desciption { get; set; }
        public string Tasks { get; set; }
        public string Hint { get; set; }
        [Display(Name = "Code Template")]
        public string CodeTemplate { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Execution Time Limit")]
        public decimal ExecutionTimeLimit { get; set; }
        [Display(Name = "Memory Limit")]
        public int MemoryLimit { get; set; }
        public string Dificulty { get; set; }
        
        [Display(Name = "Available for Practice")]
        public bool AvailableForPractice { get; set; }

        public ICollection<BatteryModel> Batteries { get; set; }
        public ICollection<SolutionModel> Solutions { get; set; }
        public ICollection<GroupContestModel> ChallengeGroups { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }

        public int ContestId { get; set; }
        public ContestModel Contest { get; set; }

    }
}
