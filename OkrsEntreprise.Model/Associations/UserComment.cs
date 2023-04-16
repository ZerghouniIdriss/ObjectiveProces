using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Model.Associations
{
    public class UserComment
    {
        public long CommentUserId { get; set; }

        public long CommentId { get; set; }

        public long UserId { get; set; }
    }
}
