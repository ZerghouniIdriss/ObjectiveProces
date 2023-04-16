using System.Collections.Generic;
using OkrsEntreprise.Model.Entities;
using OkrsEntreprise.DAL.Context;
using System.Linq;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    { 

        Comment EditComment(Comment comment, ApplicationUser user);

        Comment DeleteComment(Comment commentToDelete);
    }

    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
      

        public Comment EditComment(Comment UpdateToComment, ApplicationUser user)
        {
            using (var context = new OkrsContext())
            {
                var comment = context.Comments.FirstOrDefault(x => x.Id == UpdateToComment.Id && x.EntityCreator.Id == user.Id);

                if (comment != null)
                {
                    comment.Text = UpdateToComment.Text;                    
                    SaveContextChange(context);
                }

                return comment;
            }
        }


        public Comment DeleteComment(Comment commentToDelete)
        {
            using (var context = new OkrsContext())
            {
                var comment = context.Comments.FirstOrDefault(x => x.Id == commentToDelete.Id);

                if (comment != null)
                {
                    context.Comments.Remove(comment);
                    SaveContextChange(context);
                }
                return comment;
            }

        }
    }
}
