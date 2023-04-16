using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{
    [TenantAware("TenantId")]
    public class Indicator : EntityBase
    {
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
        public long TenantId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public List<IndicatorValue> IndicatorValues { get; set; } 
         
    }

    public class IndicatorValue 
    {
        [Key]
        public long Id { get; set; }

        public string Text { get; set; }

        [ForeignKey("IndicatorId")]
        public virtual Indicator Indicator { get; set; }
        public long IndicatorId { get; set; }



    }
}