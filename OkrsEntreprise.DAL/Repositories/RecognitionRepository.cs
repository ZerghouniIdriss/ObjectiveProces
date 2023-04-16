using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IRecognitionRepository : IRepositoryBase<Recognition>
    {
        void AddRecognitionToUsers(Recognition recognition, ApplicationUser[] users);

        void AddRecognitionToTeams(Recognition recognition, Team[] users);

        void AddRecognitionToGoals(Recognition recognition, Goal[] users);

    }

    public class RecognitionRepository : RepositoryBase<Recognition>, IRecognitionRepository
    {
        public void AddRecognitionToUsers(Recognition recognition, ApplicationUser[] users)
        {
            using (var context = new OkrsContext())
            {
                Recognition rObj = context.Recognitions.Single(r => r.Id == recognition.Id);

                context.Recognitions.Add(rObj);
                context.Recognitions.Attach(rObj);

                foreach (var user in users)
                {
                    ApplicationUser existUser = rObj.Receivers.SingleOrDefault(u => u.Id == user.Id);

                    if (existUser == null)
                    {
                        context.Users.Add(user);
                        context.Users.Attach(user);

                        rObj.Receivers.Add(user);
                    }
                }

                context.SaveChanges();
            }
        }

        public void AddRecognitionToTeams(Recognition recognition, Team[] teams)
        {
            using (var context = new OkrsContext())
            {
                Recognition rObj = context.Recognitions.Single(r => r.Id == recognition.Id);

                context.Recognitions.Add(rObj);
                context.Recognitions.Attach(rObj);

                foreach (var team in teams)
                {
                    Team existUser = rObj.TeamReceivers.SingleOrDefault(u => u.Id == team.Id);

                    if (existUser == null)
                    {
                        context.Teams.Add(team);
                        context.Teams.Attach(team);

                        rObj.TeamReceivers.Add(team);
                    }
                }

                context.SaveChanges();
            }
        }

        public void AddRecognitionToGoals(Recognition recognition, Goal[] goals)
        {
            using (var context = new OkrsContext())
            {
                Recognition rObj = context.Recognitions.Single(r => r.Id == recognition.Id);

                context.Recognitions.Add(rObj);
                context.Recognitions.Attach(rObj);

                foreach (var goal in goals)
                {
                    rObj.Goal = goal;

                    context.Goals.Add(goal);
                    context.Goals.Attach(goal);

                }
                context.SaveChanges();
            }
        }
    }
}
