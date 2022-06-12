using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication14.Models
{
    public class project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int projectId { get; set; }
        [Required]
        [StringLength(150)]
        public string projectName { get; set; }
        [Required]
        [StringLength(3000)]
        public string discription { get; set; }
        public virtual Member MemberId { get; set; }
    }
}