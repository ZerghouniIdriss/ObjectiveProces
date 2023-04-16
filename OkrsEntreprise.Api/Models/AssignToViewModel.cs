using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class AssignToViewModel
    {
        public long id { get; set; }

        public string name { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public bool isteam { get; set; }
    }
}