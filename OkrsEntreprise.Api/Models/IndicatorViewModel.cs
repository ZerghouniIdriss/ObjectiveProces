using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.Models
{
    public class IndicatorViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public List<IndicatorValueViewModel> IndicatorValues { get; set; }
    }

    public class IndicatorValueViewModel
    {
        public long value { get; set; }
        public string text { get; set; }
    }
}