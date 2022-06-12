using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int commentId { get; set; }
        public DateTime CreateDatetime { get; set; }
        [Required]
        public string Content { get; set; }
        public virtual Member MemberId { get; set; }
        public virtual project projectId { get; set; }
        public virtual Requirement reqId { get; set; }
    }
}