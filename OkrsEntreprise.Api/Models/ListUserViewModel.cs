using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class ListUserViewModel
    {
        public long Id { get; set; }

        public string Avatar { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public ListTeamViewModel[] Teams { get; set; }

        //public RoleViewModel[] Roles { get; set; }
    }

    public class RecognitionUserViewModel
    {
        public long Id { get; set; }

        public string Avatar { get; set; }

        public string UserName { get; set; }

        public TeamRecognitionViewModel[] Recognitions { get; set; }

        //public RoleViewModel[] Roles { get; set; }
    }

}