using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTACodersLicence.Models.GroupModels
{
    public class JoinGroupRequestModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public GroupModel Group { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser Solicitator { get; set; }
        [Display(Name = "Sent At")]
        public DateTime SentAt { get; set; }
    }
}
