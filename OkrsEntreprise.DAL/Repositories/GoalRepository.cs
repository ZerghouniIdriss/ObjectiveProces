using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IGoalRepository : IRepositoryBase<Goal>
    {
        void RemoveGoals(params Goal[] goals);

        void AddGoalToUser(Goal goal, ApplicationUser[] user);
        void AddGoalToTeams(Goal goal, Team[] teams);

        void AddKeyResultToGoal(Goal goalToUpdate, KeyResult goal);
        void RemoveGoalToUser(Goal goal, ApplicationUser user);

        void RemoveGoalForTeam(Goal goal, Team team);

        void UpdateStatus(long goalId, long newStatus);
        void UpdatePriority(long goalId, int priority);

        void UpdateCategory(long goalId, long newCategory);
        void UpdateTitle(long goalToUpdate, string title);
        void UpdateProgress(long goalToUpdate, long progress);
        void UpdateIsPrivate(long goalId, bool isPrivate);
        void UpdateDueDate(long goalId, DateTime? dueDate);
        void UpdateDes(long goalId, string des);
        void UpdateParent(long goalId, long? parentId);
        void AddSubGoal(long parentId, long subgoalId);
        void DeleteSubGoal(long parentId, long subgoalId);
        void UpdateIsOpen(long goalId, bool isOpen);

        IList<Goal> GetAllOpenByUserId(long id);
        IList<Goal> GetAllAlignedGoalByUserId(long id);

        IList<Goal> GetAllByTeamId(long id);
        IList<Goal> GetAllAlignedGoalByTeamId(long id);

        Task<List<Goal>> MostRecognized(int top);
        Goal GetGoalWithUsers(long goalId);


        IList<Goal> GetAllOpenAndPublic();
        IList<Goal> GetGoalsMaps();
    }

    public class GoalRepository : RepositoryBase<Goal>, IGoalRepository
    {
        public void RemoveGoals(params Goal[] goals)
        {
            using (var context = new OkrsContext())
            {
                foreach (var goal in goals)
                {
                    Goal gObj = context.Goals.FirstOrDefault(g => g.Id == goal.Id);

                    if (gObj != null)
                    {
                        List<KeyResult> krs = gObj.KeyResults.ToList();
                        krs.ForEach(k =>
                        {
                            gObj.KeyResults.Remove(k);
                        });

                        context.KeyResults.RemoveRange(krs);

                        List<Comment> comments = gObj.Comments.ToList();
                        comments.ForEach(c =>
                        {
                            gObj.Comments.Remove(c);
                        });
                        context.Comments.RemoveRange(comments);

                        List<ApplicationUser> users = gObj.Users.ToList();
                        users.ForEach(u =>
                        {
                            gObj.Users.Remove(u);
                        });

                        List<Team> teams = gObj.Teams.ToList();
                        teams.ForEach(t =>
                        {
                            gObj.Teams.Remove(t);
                        });

                        List<Goal> subgoals = gObj.SubGoals.ToList();
                        subgoals.ForEach(g =>
                        {
                            gObj.SubGoals.Remove(g);
                        });

                        List<OneOnOneSession> sessions = gObj.OneOnOneSessions.ToList();
                        sessions.ForEach(s =>
                        {
                            gObj.OneOnOneSessions.Remove(s);
                        });


                        List<PerformanceEvaluationSession> psessions = gObj.PerformanceEvaluationSessions.ToList();
                        psessions.ForEach(p =>
                        {
                            gObj.PerformanceEvaluationSessions.Remove(p);
                        });

                        Tenant tenant = context.Tenants.Where(t => t.Id == gObj.TenantId).FirstOrDefault();
                        if (tenant != null)
                        {
                            tenant.Goals.Remove(gObj);
                        }

                        GoalStatus gs = context.GoalStatus.Where(t => t.Id == gObj.GoalStatusId).FirstOrDefault();
                        if (gs != null)
                        {
                            gs.Goals.Remove(gObj);
                        }

                        GoalCategory gc = context.GoalCategory.Where(t => t.Id == gObj.GoalCategoryId).FirstOrDefault();
                        if (gc != null)
                        {
                            gc.Goals.Remove(gObj);
                        }

                        List<Activity> activites = gObj.Activities.ToList();
                        activites.ForEach(c =>
                        {
                            gObj.Activities.Remove(c);
                        });
                        context.Activities.RemoveRange(activites);

                        List<Recognition> recognitions = gObj.Recognitions.ToList();
                        recognitions.ForEach(c =>
                        {
                            gObj.Recognitions.Remove(c);
                        });
                        context.Recognitions.RemoveRange(recognitions);

                        List<GoalIndicator> goalindicators = gObj.GoalIndicators.ToList();
                        goalindicators.ForEach(c =>
                        {
                            gObj.GoalIndicators.Remove(c);
                        });
                        context.GoalIndicator.RemoveRange(goalindicators);


                        context.Entry<Goal>(gObj).State = EntityState.Deleted;

                    }
                }

                SaveContextChange(context);
            }
        }

        public void AddGoalToUser(Goal goal, ApplicationUser[] users)
        {
            using (var context = new OkrsContext())
            {
                Goal gObj = context.Goals.Single(g => g.Id == goal.Id);

                context.Goals.Add(gObj);
                context.Goals.Attach(gObj);

                foreach (var user in users)
                {
                    ApplicationUser existUser = gObj.Users.SingleOrDefault(u => u.Id == user.Id);

                    if (existUser == null)
                    {
                        context.Users.Add(user);
                        context.Users.Attach(user);

                        gObj.Users.Add(user);
                    }
                }

                context.SaveChanges();
            }
        }

        //public void Add(Goal goal)
        //{
        //    using (var context = new OkrsContext())
        //    {
        //        Goal gObj = context.Goals.Single(g => g.Id == goal.Id);

        //        context.Goals.Add(gObj);
        //        context.Goals.Attach(gObj);

        //        foreach (var user in users)
        //        {
        //            ApplicationUser existUser = gObj.Users.SingleOrDefault(u => u.Id == user.Id);

        //            if (existUser == null)
        //            {
        //                context.Users.Add(user);
        //                context.Users.Attach(user);

        //                gObj.Users.Add(user);
        //            }
        //        }

        //        context.SaveChanges();
        //    }
        //}

        public void AddGoalToTeams(Goal goal, Team[] teams)
        {

            using (var context = new OkrsContext())
            {
                Goal gObj = new Goal() { Id = goal.Id };

                context.Goals.Add(gObj);
                context.Goals.Attach(gObj);

                foreach (var team in teams)
                {
                    context.Teams.Add(team);
                    context.Teams.Attach(team);

                    gObj.Teams.Add(team);
                }

                context.SaveChanges();
            }
        }

        public void RemoveGoalToUser(Goal goal, ApplicationUser user)
        {

            using (var context = new OkrsContext())
            {
                Goal gObj = context.Goals.FirstOrDefault(g => g.Id == goal.Id);

                if (gObj != null)
                {
                    var usr = gObj.Users.FirstOrDefault(u => u.Id == user.Id);

                    if (usr != null)
                    {
                        gObj.Users.Remove(usr);
                    }

                    // call SaveChanges
                    SaveContextChange(context);
                }
            }
        }

        public void RemoveGoalForTeam(Goal goal, Team team)
        {

            using (var context = new OkrsContext())
            {
                Goal gObj = context.Goals.FirstOrDefault(g => g.Id == goal.Id);

                if (gObj != null)
                {
                    var tm = gObj.Teams.FirstOrDefault(u => u.Id == team.Id);

                    if (tm != null)
                    {
                        gObj.Teams.Remove(tm);
                    }

                    // call SaveChanges
                    SaveContextChange(context);
                }
            }
        }

        public void AddKeyResultToGoal(Goal goalToUpdate, KeyResult keyresultToAdd)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalToUpdate.Id);
                goal.KeyResults.Add(keyresultToAdd);
                context.KeyResults.Add(keyresultToAdd);
                SaveContextChange(context);
            }
        }

        public void UpdateStatus(long goalToUpdate, long newStatus)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalToUpdate);
                var status = context.GoalStatus.First(x => x.Id == newStatus);
                goal.GoalStatus = status;
                goal.GoalStatus.Id = status.Id;
                SaveContextChange(context);
            }
        }

        public void UpdatePriority(long goalId, int priority)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalId);
                goal.Priority = priority;
                SaveContextChange(context);
            }
        }

        public void UpdateCategory(long goalToUpdate, long newCategory)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalToUpdate);
                var category = context.GoalCategory.First(x => x.Id == newCategory);
                goal.GoalCategory = category;
                goal.GoalCategory.Id = category.Id;
                SaveContextChange(context);
            }
        }

        public void UpdateTitle(long goalToUpdate, string title)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalToUpdate);
                goal.Title = title;
                SaveContextChange(context);
            }
        }

        public void UpdateIsPrivate(long goalId, bool isPrivate)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalId);
                goal.IsPrivate = isPrivate;
                SaveContextChange(context);
            }
        }

        public void UpdateDueDate(long goalId, DateTime? dueDate)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalId);
                goal.Deadline = dueDate;
                SaveContextChange(context);
            }
        }

        //public void UpdateDes(long goalId, string des)
        //{
        //    using (var context = new OkrsContext())
        //    {
        //        var goal = context.Goals.First(x => x.Id == goalId);
        //        goal.Detail = des;
        //        SaveContextChange(context);
        //    }
        //}

        public void UpdateDes(long goalId, string des)
        {
            var goalToUpdate = new Goal() { Id = goalId, Detail = des };
            Update(goalToUpdate, x => x.Detail);
        }

        public void UpdateProgress(long goalId, long progress)
        {
            var goalToUpdate = new Goal() { Id = goalId, Progress = progress };
            Update(goalToUpdate, x => x.Progress);
        }

        public void UpdateIsOpen(long goalId, bool isOpen)
        {
            var goalToUpdate = new Goal() { Id = goalId, IsOpen = isOpen };
            Update(goalToUpdate, x => x.IsOpen);
        }

        public void UpdateParent(long goalId, long? parentId)
        {
            //bool isUpdated = false;

            using (var context = new OkrsContext())
            {
                var goal = context.Goals.First(x => x.Id == goalId);
                if (goal != null)
                {
                    if (parentId.HasValue)
                    {
                        var parentgoal = context.Goals.FirstOrDefault(x => x.Id == parentId.Value);

                        if (parentgoal == null)
                        {
                            throw new Exception("No objective found to set as parent");
                        }

                        //1 - parent goal should not be same as the goal be assign
                        if (goalId == parentId.Value)
                        {
                            throw new Exception("Parent and assignee objective is same");
                        }

                        //2 - check if parent goal is not in subgoals of the goal being assiged
                        if (goal.SubGoals.Contains(parentgoal))
                        {
                            throw new Exception("Parent objective found already in child objectives");
                        }

                        goal.ParentId = parentId;
                        //isUpdated = true;
                    }
                    else
                    {
                        goal.ParentId = null;
                    }

                    SaveContextChange(context);
                }
            }
        }

        public void AddSubGoal(long parentId, long subgoalId)
        {
            using (var context = new OkrsContext())
            {
                var pargoal = context.Goals.First(x => x.Id == parentId);
                var subgoal = context.Goals.First(x => x.Id == subgoalId);

                if (pargoal != null && subgoal != null)
                {
                    //check if subgoal is not the parent of the goal being added
                    if (pargoal.Id == subgoal.Id)
                    {
                        throw new Exception("Parent and child objective is same");
                    }

                    if (pargoal.SubGoals.Contains(subgoal))
                    {
                        throw new Exception("Child objective already found");
                    }

                    context.Goals.Attach(subgoal);
                    pargoal.SubGoals.Add(subgoal);

                    SaveContextChange(context);
                }
            }
        }

        public void DeleteSubGoal(long parentId, long subgoalId)
        {
            using (var context = new OkrsContext())
            {
                var pargoal = context.Goals.FirstOrDefault(x => x.Id == parentId);

                if (pargoal != null)
                {
                    var subgoal = pargoal.SubGoals.FirstOrDefault(x => x.Id == subgoalId);
                    if (subgoal != null)
                    {
                        pargoal.SubGoals.Remove(subgoal);
                    }
                    SaveContextChange(context);
                }
            }
        }

       
        public IList<Goal> GetAllAlignedGoalByUserId(long id)
        {
            return base.GetList(g => g.ParentId != null && g.Users.Any(u => u.Id == id) && g.IsOpen);
        }

        public IList<Goal> GetAllAlignedGoalByTeamId(long id)
        {
            return base.GetList(g => g.ParentId != null && g.Teams.Any(t => t.Id == id) && g.IsOpen && !g.IsPrivate);
        }

        public IList<Goal> GetAllByTeamId(long id)
        {
            return GetList(x => x.Teams.Any(t => t.Id == id) && x.IsOpen && !x.IsPrivate);
        }

        public Task<List<Goal>> MostRecognized(int top)
        {
            return Task.Run(() =>
            {
                using (var context = new OkrsContext())
                {
                    return context
                        .Goals
                        .Include(x => x.KeyResults)
                        .Include(x => x.GoalStatus)
                        .Include(x => x.GoalCategory)
                        .Include(x => x.EntityCreator)
                        .Include(x => x.Comments)
                        .Include(x => x.Comments.Select(y => y.EntityCreator))
                        .Include(x => x.Parent)
                        .Include(x => x.SubGoals)
                        .Include(x => x.Users)
                        .Include(x => x.Teams)
                        .Include(x => x.Recognitions)
                        .Where(x => x.IsOpen && !x.IsPrivate)
                        .OrderByDescending(x => x.CreatedDate)
                        .OrderByDescending(g => g.Recognitions.Count)
                        .Take(top)
                        .ToList();
                }
            });
        }

        public Goal GetGoalWithUsers(long goalId)
        {
            return this.GetSingle(x => x.Id == goalId, x => x.Users);
        }

        public IList<Goal> GetAllOpenByUserId(long id)
        {
            return GetList(x => x.Users.Any(u => u.Id == id) && x.IsOpen);
        }

        public IList<Goal> GetAllOpenAndPublic()
        {
            return GetList(x => x.IsOpen && !x.IsPrivate);
        }

        public IList<Goal> GetGoalsMaps()
        {
            return GetList(x => x.IsOpen && !x.IsPrivate,
                    x => x.GoalStatus,
                    x => x.Parent, x => x.SubGoals, x => x.Users, x => x.Teams)
                .OrderByDescending(x => x.CreatedDate).ToList();
        }
    }

}
