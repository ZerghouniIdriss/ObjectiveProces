using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class GoalMapViewModel
    {
        public  long id { get; set; }
        public string title { get; set; }
        public long? ParentId { get; set; }
        public AssignToViewModel[] users { get; set; } 
        public AssignToViewModel[] teams { get; set; } 
        public GoalCategoryViewModel category { get; set; } 
        public DateTime? duedate { get; set; }
        public GoalStatusViewModel status { get; set; } 
        public string description { get; set; } 
        public bool isprivate { get; set; } 
        public int progress { get; set; }  
        public bool isaligned { get; set; }
        public bool isopen { get; set; }

        public int priority { get; set; }

    }
}