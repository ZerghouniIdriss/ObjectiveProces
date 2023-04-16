using OkrsEntreprise.DAL.Repositories;
using OkrsEntreprise.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using OkrsEntreprise.Framework;

namespace OkrsEntreprise.Services
{
    public interface ICommentsService: IServiceBase<Comment>
    {
        void Add(Comment comment);
        Comment EditComment(Comment comment, ApplicationUser user);
        Comment DeleteComment(Comment commentToDelete);
    }

   

    public class CommentsService : CRUDServiceBase<CommentRepository, Comment>, ICommentsService
    {
        private ICommentRepository _commentRepository;
        private IGoalRepository _goalRepository;
        private IActivityService _activityService;
        private IEmailService _emailService;
        private ICurrentContextProvider<ApplicationUser> _currentContextProvider;

        public CommentsService(ICommentRepository commentRepository, IGoalRepository goalRepository, IActivityService activityService, ICurrentContextProvider<ApplicationUser> currentContextProvider,
            IEmailService emailService)
        {
            _commentRepository = commentRepository;
            _goalRepository =goalRepository;
            _activityService = activityService;
            _emailService = emailService;
            _currentContextProvider=currentContextProvider;
        } 

        public override void Add(Comment comment)
        {
            var currentUserId= _currentContextProvider.GetCurrentUser().Id;
            comment.EntityCreatorId = currentUserId;
            _commentRepository.Add(comment);

            var goal = _goalRepository.GetGoalWithUsers(comment.GoalId);

            var activity = new Activity()
            {
                ActorId = currentUserId,
                ActionText = "Added new comment on",
                GoalId = goal.Id
            };
            _activityService.Add(activity);

            foreach (var recepient in goal.Users)
            {
                if (!string.IsNullOrEmpty(recepient.Email))
                {
                    _emailService.SendEmail(recepient.Email, "New Comment added on " + goal.ToString(), comment.Text);
                }

            }
        }

        public Comment EditComment(Comment comment, ApplicationUser user)
        {
            var result = _commentRepository.EditComment(comment, user);
            return result;           
        }

        public Comment DeleteComment(Comment commentToDelete)
        {
            var result =_commentRepository.DeleteComment(commentToDelete);
            return result;
        }
    }
}
