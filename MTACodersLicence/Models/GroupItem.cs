using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MTACodersLicence.Models
{
    public class GroupItem
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public DateTime AddeDateTime { get; set; }
        public int GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual GroupModel Group { get; set; }
    }
}
