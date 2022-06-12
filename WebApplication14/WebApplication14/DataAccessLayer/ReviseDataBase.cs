using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebApplication14.Models;

namespace WebApplication14.DataAccessLayer
{
    public class ReviseDataBase : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MemberScore> MemberScores { get; set; }
        public DbSet<projectMember> ProjectMembers { get; set; }
        public DbSet<RequirementType> RequirementTypes { get; set; }
        public DbSet<ComplitionScore> ComplitionScores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Member>().ToTable("Members");
           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        
    
      
    }


}