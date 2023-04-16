using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class UserEditViewModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ManagerUserName { get; set; }

        public long ManagerId { get; set; }

        public TeamViewModel[] Teams { get; set; }

        public RoleViewModel[] Roles { get; set; }
    }
}