using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication14.DataAccessLayer;


namespace WebApplication14.Models
{
    public class Requirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reqId { get; set; }
        [Required]
        [StringLength(150)]
        public string title { get; set; }
        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime craetedDateTime { get; set; }
        [Required]
        [StringLength(3000)]
        public string discription { get; set; }
        public int Total_Complition_Score { get; set; }
        public bool isActive { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime endDatetime { get; set; }
        [Range(0, 100)]
        public int thresholdScore { get; set; }
        public bool isAprroved { get; set; }
        public double thresholdScoreValue { get; set; }
        public virtual RequirementType typeId { get; set; }
        public virtual project projectId { get; set; }
    
         
    public bool calMemberScore(ReviseDataBase db , Member member)
        {
            if ( db.MemberScores.Where(m => m.MemberId.MemberId == member.MemberId).Where(sq => sq.reqId.reqId == this.reqId).Count() == 0 && this.isActive == false)
            {
                MemberScore memberScore = new MemberScore { MemberId = member, reqId = this , description = "standard requirment scoring", createDateTime = DateTime.Now, ScoreValue = db.Comments.Where(c => c.MemberId.MemberId == member.MemberId && c.reqId.reqId == this.reqId).Count() };
                db.MemberScores.Add(memberScore);
                db.SaveChanges();

                member.scoreMember = db.MemberScores.Where(m => m.MemberId.MemberId == member.MemberId).Sum(s => s.ScoreValue);
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                if (member.scoreMember<50)
                {
                    member.grade = 1;
                }
                else if(member.scoreMember<100)
                {
                    member.grade = 2;
                }
                else if(member.scoreMember < 150)
                {
                    member.grade = 3;
                }
                else
                {
                    member.grade = 4;
                }
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else { return false; }

        }
    }


}