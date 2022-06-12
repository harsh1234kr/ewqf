using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WebApplication14.Models
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }
        [Required]
        [StringLength(150)]
        public string firstName { get; set; }
        [Required]
        [StringLength(150)]
        public string lastName { get; set; }
        [Required]
        
        [StringLength(150)]
        
        public string userName { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [StringLength(12,MinimumLength =7)]
        public string phone { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string password { get; set; }

        public bool IsAdmin { get; set; }
        public string pic { get; set; }
        public int scoreMember { get; set; }
        public int grade { get; set; }
    }
}