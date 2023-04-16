using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{
    public class KeyResult : EntityBase
    {
        public string Title { get; set; }
        public string Status { get; set; }

        [ForeignKey("GoalId")]
        public virtual Goal Goal { get; set; }
        public virtual long GoalId { get; set; }

        public override string ToString()
        {
            return "#" + Id + "-" + Title;
        }
    }
}