using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{
   public class GoalIndicator
    {
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }
        public long TenantId { get; set; }

        [Key,ForeignKey("Goal"),Column(Order = 1)]
        public long GoalId { get; set; } 

        [Key,ForeignKey("Indicator"), Column(Order = 2)]
        public long IndicatorId { get; set; }

        [Key, ForeignKey("IndicatorValue"), Column(Order = 3)]
        public long IndicatorValueId { get; set; }

        public virtual Goal Goal { get; set; }
        public virtual Indicator Indicator { get; set; }
        public virtual IndicatorValue IndicatorValue { get; set; } 

    }
    
}
