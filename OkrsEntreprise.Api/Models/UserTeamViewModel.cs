using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class UserTeamViewModel
    {
        public long Id { get; set; }

        public string Avatar { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

    }
}