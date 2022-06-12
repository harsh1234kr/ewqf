using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class ComplitionScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComplitionScoreId { get; set; }
        public int scoreValue { get; set; }
        public virtual Member MemberId { get; set; }
        public virtual Requirement reqId { get; set; }
    }
}