using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class MemberScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int memberScoreId { get; set; }
        [Required]
        public DateTime createDateTime { get; set; }
        [Required]
        public int ScoreValue { get; set; }
        public string description { get; set; }
        public virtual Member MemberId { get; set; }
        public virtual Requirement reqId { get; set; }

    }
}