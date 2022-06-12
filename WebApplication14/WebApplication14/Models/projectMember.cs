using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class projectMember
    {
        [Key]
        public DateTime joinDate { get; set; }
        public virtual project projectId { get; set; }
        public virtual Member MemberId { get; set; }
        public virtual Role roleId { get; set; }
    }
}