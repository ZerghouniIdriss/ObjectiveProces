using System.ComponentModel.DataAnnotations.Schema;

namespace OkrsEntreprise.Model.Entities
{
    public class Comment : EntityBase
    {
        public string Text { get; set; }

        [ForeignKey("GoalId")]
        public virtual Goal Goal { get; set; }
        public virtual long GoalId { get; set; }

    }
}
