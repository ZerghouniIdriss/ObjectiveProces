using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using OkrsEntreprise.Model.Entities;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using OkrsEntreprise.Model.Associations;
using OkrsEntreprise.Model.Entities.Surveys;

namespace OkrsEntreprise.DAL.Context
{
    public interface IOkrsContext :IDbContextFactory<OkrsContext>
    { 
    }

    public class OkrsContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IOkrsContext
    {
        public  OkrsContext Create()
        {
            return new OkrsContext();
        }
        public OkrsContext() : base("OkrsEntreprise")
        {
        }
         
        public OkrsContext(string dbContextName) : base(dbContextName)
        {
        }


        public DbSet<Goal> Goals { get; set; }

        public DbSet<KeyResult> KeyResults { get; set; }

        public DbSet<GoalStatus> GoalStatus { get; set; }

        public DbSet<GoalCategory> GoalCategory { get; set; }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<PerformanceEvaluationSession> PerformanceEvaluationSessions { get; set; }
        public DbSet<PerformanceSurveyUserAnswer> PerformanceSurveyUserAnswer { get; set; }

        public DbSet<OneOnOneSession> OneOnOneSessions { get; set; }


        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> Questions { get; set; }
        public DbSet<SurveyOption> Options { get; set; } 

        public DbSet<SessionStatus> SessionStatus { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Activity> Activities { get; set; } 
         
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<Recognition> Recognitions { get; set; }

        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<IndicatorValue> IndicatorValues { get; set; }

        public DbSet<GoalIndicator> GoalIndicator { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            var conv = new AttributeToTableAnnotationConvention<TenantAwareAttribute, string>(
                  TenantAwareAttribute.TenantAnnotation, (type, attributes) => attributes.Single().ColumnName);
            modelBuilder.Conventions.Add(conv);


            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles") ;
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            

            //UserGoals
            modelBuilder.Entity<ApplicationUser>().
                HasMany(c => c.Goals).
                WithMany(p => p.Users).
                Map(
                    m =>
                    {
                        m.MapLeftKey("UserId");
                        m.MapRightKey("GoalId");
                        m.ToTable("UserGoals");
                    });

            //TeamGoals
            modelBuilder.Entity<Team>().
                HasMany(c => c.Goals).
                WithMany(p => p.Teams).
                Map(
                    m =>
                    {
                        m.MapLeftKey("TeamId");
                        m.MapRightKey("GoalId");
                        m.ToTable("TeamGoals");
                    });

            //UserTeams
            modelBuilder.Entity<ApplicationUser>().
                HasMany(c => c.Teams).
                WithMany(p => p.Users).
                Map(
                    m =>
                    {
                        m.MapLeftKey("UserId");
                        m.MapRightKey("TeamId");
                        m.ToTable("UserTeams");
                    });


            //Goal, SubGoals
            modelBuilder.Entity<Goal>()
                .HasOptional(i => i.Parent)
                .WithMany(i => i.SubGoals)
                .HasForeignKey(i => i.ParentId);


            //OneOnOneSessionGoals
            modelBuilder.Entity<OneOnOneSession>()
                  .HasMany<Goal>(s => s.Goals)
                  .WithMany(c => c.OneOnOneSessions)
                  .Map(cs =>
                  {
                      cs.MapLeftKey("OneOnOneSession_Id");
                      cs.MapRightKey("Goal_Id");
                      cs.ToTable("OneOnOneSessionGoals");
                  });


            //RecognitionReceivers
            modelBuilder.Entity<Recognition>().
                HasMany(c => c.Receivers).
                WithMany(p => p.Recognitions).
                Map(
                    m =>
                    {
                        m.MapLeftKey("RecognitionId");
                        m.MapRightKey("NomineeId");
                        m.ToTable("RecognitionReceivers");
                    });


            //RecognitionTeamReceivers
            modelBuilder.Entity<Recognition>().
                HasMany(c => c.TeamReceivers).
                WithMany(p => p.Recognitions).
                Map(
                    m =>
                    {
                        m.MapLeftKey("RecognitionId");
                        m.MapRightKey("TeamId");
                        m.ToTable("RecognitionTeamReceivers");
                    });

        }



    }
}
