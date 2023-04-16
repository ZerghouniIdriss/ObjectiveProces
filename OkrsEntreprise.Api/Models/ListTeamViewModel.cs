using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class ListTeamViewModel
    {
        public int id { set; get; }
        public string name { set; get; }
        public string description { get; set; }
        public string Avatar { get; set; }
    }
}