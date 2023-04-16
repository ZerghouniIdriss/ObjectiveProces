//using Microsoft.AspNet.Identity;
//using OkrsEntreprise.Model.Entities;
//using OkrsEntreprise.Model.Entities.Surveys;
//using System;
//using System.Collections.Generic;
//using System.Linq;



//namespace OkrsEntreprise.DAL.Context
//{
//    [System.Obsolete("Use this only to test the porformance. use the ordinary initializer in stead")]
//    public class OkrsHeavyInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OkrsContext>
//    {
//        private Goal _goal1;
//        private Goal _goal2;
//        private Goal _goal3;
//        private ApplicationUser _user1;
//        private ApplicationUser _user2;
//        private Team _team1;
//        private Team _team2;

//        protected override void Seed(OkrsContext context)
//        {

//            #region Tenant
//            InitializeTenants(context);
//            #endregion

//            #region ApplicationUser
//            _user1 = new ApplicationUser() { Email = "user1@okrs.com", FirstName = "user1", UserName = "user1", TenantId = 1 };
//            _user2 = new ApplicationUser() { Email = "user2@okrs.com", FirstName = "user2", UserName = "user2", TenantId = 1 };
//            List<ApplicationUser> users = new List<ApplicationUser>
//            {
//                _user1,
//                _user2
//            };
//            #endregion

//            #region Team
//            _team1 = new Team() { Name = "Team1", TenantId = 1 };
//            _team2 = new Team() { Name = "Team2", TenantId = 1 };
//            List<Team> teams = new List<Team>
//            {
//                _team1,
//                _team2
//            };
//            #endregion

//            #region GoalStatus
//            List<GoalStatus> goalStatus = new List<GoalStatus>
//            {
//                new GoalStatus{StatusTitle= "New"},
//                new GoalStatus{StatusTitle="In Progress"},
//                new GoalStatus{StatusTitle="Achieved"}
//            };
//            goalStatus.ForEach(s => context.GoalStatus.Add(s));
//            context.SaveChanges();
//            #endregion

//            #region GoalCategory
//            List<GoalCategory> goalCategory = new List<GoalCategory>
//            {
//                new GoalCategory{CategoryTitle="Personal"},
//                new GoalCategory{CategoryTitle= "General"},
//                new GoalCategory{CategoryTitle="1-On-1s"}
//            };
//            goalCategory.ForEach(s => context.GoalCategory.Add(s));
//            context.SaveChanges();
//            #endregion

//            #region KeyResults
//            List<KeyResult> KeyResults1 = new List<KeyResult>
//            {
//                new KeyResult {Title = "Key result 1"   },
//                new KeyResult  {Title = "Key result 2" },
//            };

//            List<KeyResult> KeyResults2 = new List<KeyResult>
//            {
//                new KeyResult {Title = "Key result 1"   },
//                new KeyResult  {Title = "Key result 2" },
//            };

//            List<KeyResult> KeyResults3 = new List<KeyResult>
//            {
//                new KeyResult  {Title = "Key result 1" },
//                new KeyResult  {Title = "Key result 2" },
//                new KeyResult  {Title = "Key result 3" },
//                new KeyResult  {Title = "Key result 4" }
//            };

//            List<KeyResult> TeamKeyResults = new List<KeyResult>
//            {
//                new KeyResult  {Title = "Key Result 1" },
//                new KeyResult  {Title = "Key Result 2" },
//                new KeyResult  {Title = "Key Result 3" },
//                new KeyResult  {Title = "Key Result 4" }
//            };
//            #endregion

//            #region Goals 
//            _goal1 = new Goal
//            {
//                GoalStatusId = 1,
//                GoalCategoryId = 1,
//                Title = "Goal number i, For user1",
//                KeyResults = new List<KeyResult>(KeyResults1),
//                Users = new List<ApplicationUser>() { _user1 },
//                TenantId = 1,
//            };

//            _goal2 = new Goal
//            {
//                GoalStatusId = 1,
//                GoalCategoryId = 1,
//                Title = "Goal number 2, For user2",
//                KeyResults = new List<KeyResult>(KeyResults2),
//                Users = new List<ApplicationUser>() { _user2 },
//                TenantId = 1,
//            };

//            _goal3 = new Goal
//            {
//                GoalStatusId = 1,
//                GoalCategoryId = 1,
//                Title = "Goal number 3, for both users",
//                KeyResults = new List<KeyResult>(KeyResults3),
//                Users = new List<ApplicationUser>() { _user1, _user2 },
//                TenantId = 1,
//            };

//            var goal4 = new Goal
//            {
//                GoalStatusId = 1,
//                GoalCategoryId = 1,
//                Title = "Goal number 4, for Team1",
//                KeyResults = new List<KeyResult>(TeamKeyResults),
//                Teams = new List<Team>() { _team1 },
//                TenantId = 1,
//            };

//            var goal5 = new Goal
//            {
//                GoalStatusId = 1,
//                GoalCategoryId = 1,
//                Title = "Goal number 5, For Team2",
//                KeyResults = new List<KeyResult>(TeamKeyResults),
//                Teams = new List<Team>() { _team2 },
//                TenantId = 1,
//            };

//            var goal6 = new Goal
//            {
//                GoalStatusId = 1,
//                GoalCategoryId = 1,
//                Title = "Goal number 4, For Team1 and Team2",
//                KeyResults = new List<KeyResult>(TeamKeyResults),
//                Teams = new List<Team>() { _team1, _team2 },
//                TenantId = 1,
//            };

//            List<Goal> goals = new List<Goal>
//            {_goal1, _goal2, _goal3,goal4,goal5,goal6};
//            goals.ForEach(s => context.Goals.Add(s));
//            context.SaveChanges();
//            #endregion

//            #region SessionStatus
//            List<SessionStatus> sessionStatus = new List<SessionStatus>
//            {
//                new SessionStatus{Title= "ToDo"},
//                new SessionStatus{Title="Completed"},
//                new SessionStatus{Title="Reviewd"}
//            };
//            sessionStatus.ForEach(s => context.SessionStatus.Add(s));
//            context.SaveChanges();
//            #endregion

//            #region oneOnOneSessoin    
//            var oneOnOneSessoin = new OneOnOneSession()
//            {
//                Animator = _user1,
//                Attendee = _user2,
//                Goals = new List<Goal> { _goal1, _goal2, _goal3 },
//                SessionStatusId = 1
//            };
//            context.OneOnOneSessions.Add(oneOnOneSessoin);
//            context.SaveChanges();
//            #endregion

//            #region performanceEvaluationSession
//            InitializeSurvey(context);

//            var performanceEvaluationSession1 = new PerformanceEvaluationSession()
//            {
//                Animator = _user1,
//                Attendee = _user2,
//                Goals = new List<Goal> { _goal1, _goal2, _goal3 },
//                SessionStatusId = 1,
//                PerformanceSurveyId = context.Surveys.FirstOrDefault(x => x.Code == "PERF_EVALUATION_SESSION").Id,
//            };
//            context.PerformanceEvaluationSessions.Add(performanceEvaluationSession1);
//            context.SaveChanges();

//            var serveyUserAnswer1 = new PerformanceSurveyUserAnswer()
//            {
//                SurveyId = context.Surveys.FirstOrDefault(x => x.Code == "PERF_EVALUATION_SESSION").Id,
//                SurveyQuestionId = 1,
//                SurveyOptionId = 1,
//                UserId = 1,
//                PerformanceEvaluationSessionId = 1
//            };
//            context.PerformanceSurveyUserAnswer.Add(serveyUserAnswer1);
//            context.SaveChanges();

//            #endregion

//            #region Activities
//            InitializeActivities(context);
//            #endregion

//            #region Roles
//            InitializeRoles(context);
//            #endregion

//            #region Contents
//            InitializeContents(context);
//            #endregion

//            #region Admin
//            InitializeAdmin(context);
//            #endregion

//            #region Recognition
//            InitializeRecognition(context);
//            #endregion

//            #region Recognition
//            HeavyInitialization(1000, context);
//            #endregion

//        }


//        private void InitializeRoles(OkrsContext context)
//        {
//            List<ApplicationRole> roles = new List<ApplicationRole>()
//            {
//                new ApplicationRole("Employee") ,
//                new ApplicationRole("Manager")  ,
//                new ApplicationRole("Admin") ,
//                new ApplicationRole("SysAdmin")
//            };
//            roles.ForEach(s => context.Roles.Add(s));
//            context.SaveChanges();

//        }

//        private void InitializeAdmin(OkrsContext context)
//        {
//            var passwordHash = new PasswordHasher();
//            var user = new ApplicationUser()
//            {
//                PasswordHash = passwordHash.HashPassword("admin2016"),
//                Email = "zerghouni.idriss@gmail.com",
//                UserName = "admin",
//                SecurityStamp = Guid.NewGuid().ToString(),
//                TenantId = 1,
//            };

//            var adminRole = context.Roles.FirstOrDefault(x => x.Name == "SysAdmin");
//            if (adminRole != null) user.Roles.Add(new ApplicationUserRole() { UserId = user.Id, RoleId = adminRole.Id });
//            context.Users.Add(user);
//            context.SaveChanges();
//        }

//        private void InitializeActivities(OkrsContext context)
//        { 
//            var activity = new Activity()
//            {  
//                Actor = _user1,
//                ActionText = "Added new comment on",
//                Goal =_goal1,
                 
//            }; 
//            context.Activities.Add(activity);
//            context.SaveChanges();

//        }

//        private void InitializeTenants(OkrsContext context)
//        {
//            var tenant1 = new Tenant() { Name = "Okrs" };
//            var tenant2 = new Tenant() { Name = "XYZCompany" };
//            var tenant3 = new Tenant() { Name = "ABCCompany" };
//            var tenants = new List<Tenant>() { tenant1, tenant2, tenant3 };
//            tenants.ForEach(s => context.Tenants.Add(s));
//            context.SaveChanges();

//        }

//        private void InitializeContents(OkrsContext context)
//        {
//            var content1 = new Content() { Code = "C1", Text = "Content 1" };
//            var content2 = new Content() { Code = "C2", Text = "Content 2" };
//            var content3 = new Content() { Code = "C3", Text = "Content 3" };
//            var contents = new List<Content>() { content1, content2, content3 };
//            contents.ForEach(s => context.Contents.Add(s));
//            context.SaveChanges();

//        }

//        private void InitializeSurvey(OkrsContext context)
//        {
//            Survey survey1 = new Survey()
//            {
//                Code = "PERF_EVALUATION_SESSION"
//            };
//            context.Surveys.Add(survey1);
//            context.SaveChanges();

//            SurveyQuestion question1 = new SurveyQuestion()
//            {
//                Text = "Job Knowledge",
//                SurveyId = survey1.Id
//            };

//            SurveyQuestion question2 = new SurveyQuestion()
//            {
//                Text = "Productivity",
//                SurveyId = survey1.Id
//            };

//            SurveyQuestion question3 = new SurveyQuestion()
//            {
//                Text = "WorkQuality",
//                SurveyId = survey1.Id
//            };

//            SurveyQuestion question4 = new SurveyQuestion()
//            {
//                Text = "Technical Skills",
//                SurveyId = survey1.Id
//            };

//            SurveyQuestion question5 = new SurveyQuestion()
//            {
//                Text = "Work Concistency",
//                SurveyId = survey1.Id
//            };
//            List<SurveyQuestion> questions = new List<SurveyQuestion>() { question1, question2, question3, question4, question5 };
//            questions.ForEach(s => context.Questions.Add(s));
//            context.SaveChanges();


//            SurveyOption option1 = new SurveyOption()
//            {
//                Text = "Excellent",
//                SurveyId = survey1.Id,

//            };

//            SurveyOption option2 = new SurveyOption()
//            {
//                Text = "Good",
//                SurveyId = survey1.Id
//            };

//            SurveyOption option3 = new SurveyOption()
//            {
//                Text = "Fair",
//                SurveyId = survey1.Id
//            };

//            SurveyOption option4 = new SurveyOption()
//            {
//                Text = "Poor",
//                SurveyId = survey1.Id
//            };
//            List<SurveyOption> options = new List<SurveyOption>() { option1, option2, option3, option4 };
//            options.ForEach(s => context.Options.Add(s));
//            context.SaveChanges();

//        }

//        private void InitializeRecognition(OkrsContext context)
//        {
//            var recognition = new Recognition()
//            {
//                Giver = _user1,
//                Receivers = new List<ApplicationUser>() { _user2, _user1 },
//                Goal = _goal1,
//                Text = "Thanks for helping me acheiving my objective" + _goal1,
//                TenantId = 1,
//                TeamReceivers = new List<Team>() { _team1, _team2 }
//            };


//            context.Recognitions.Add(recognition);
//            context.SaveChanges();
//        }

//        private void HeavyInitialization(int size, OkrsContext context)
//        {

//            var userList = new List<ApplicationUser>() { _user1, _user2 };
//            var teamList = new List<Team>() { _team1, _team2 };

//            var keyResultlist = new List<KeyResult>();
//            var commentList = new List<Comment>();
//            var oneOnOneSessionList = new List<OneOnOneSession>();
//            var performanceEvaluationList = new List<PerformanceEvaluationSession>();
//            var recognitionList = new List<Recognition>();

//            var subgoalsList = new List<Goal>();
//            var goalList = new List<Goal>();

//            var passwordHash = new PasswordHasher();
//            for (int i = 0; i < size / 2; i++)
//            {
//                var tempUser = new ApplicationUser()
//                {
//                    Email = "user" + "i" + "@okrs.com",
//                    PasswordHash = passwordHash.HashPassword("useri"),
//                    SecurityStamp = Guid.NewGuid().ToString(),
//                    FirstName = "user" + i,
//                    UserName = "user" + i,
//                    TenantId = 1
//                };
//                userList.Add(tempUser);
//            }
//            userList.ForEach(s => context.Users.Add(s));

//            for (int i = 0; i < size / 10; i++)
//            {
//                var tempkeyresult = new KeyResult();
//                { };
//                keyResultlist.Add(tempkeyresult);
//            }
//            keyResultlist.ForEach(s => context.KeyResults.Add(s));


//            for (int i = 0; i < size / 10; i++)
//            {
//                var tempcomment = new Comment()
//                {
//                    Text = "Comment" + i
//                };
//                commentList.Add(tempcomment);
//            }
//            commentList.ForEach(s => context.Comments.Add(s));


//            for (int i = 0; i < 5; i++)
//            {
//                var tempgoal = new Goal
//                {
//                    GoalStatusId = 1,
//                    GoalCategoryId = 1,
//                    Title = "Heavy Goal number" + i,
//                    KeyResults = keyResultlist,
//                    TenantId = 1,
//                    Users = userList,
//                    Teams = teamList,
//                    IsOpen = true,
//                    IsPrivate = false,
//                    Comments = commentList
//                };

//                goalList.Add(tempgoal);
//            }

//            goalList.ForEach(s => context.Goals.Add(s));
//            context.SaveChanges();
//        }

//    }
//}
