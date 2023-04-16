using System.Collections.Generic;

namespace OkrsEntreprise.Api.Models
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string ManagerUserName { get; set; }
        public long ManagerId { get; set; }

        public TeamViewModel[] Teams { get; set; }

        public RoleViewModel[] Roles { get; set; }

        public SessionGoalViewModel[] Goals { get; set; }

        public virtual List<ListGoalFullLoadViewModel> Objective { get; set; }

        public RecognitionViewModel[] Recognitions { get; set; }
        public IEnumerable<ListGoalFullLoadViewModel> PrivateGoals { get; set; }
    }
}