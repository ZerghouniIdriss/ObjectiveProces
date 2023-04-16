using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class InviteUsersViewModel
    {
        public long userId { get; set; }

        public string[] emails { get; set; }
    }
}