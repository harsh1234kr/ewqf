using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class ChatUserDetails
    {
        [StringLength(7, MinimumLength = 2, ErrorMessage = "UserName length should be between 2 and 7")]
        public int MemberId { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public int ReqId { get; set; }
    }
}