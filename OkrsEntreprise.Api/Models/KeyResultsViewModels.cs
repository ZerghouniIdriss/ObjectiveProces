using System.ComponentModel.DataAnnotations;

namespace OkrsEntreprise.Api.Models
{
    public class KeyResultViewModel
    {
        public long id { get; set; }
        public long goalid { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, ErrorMessage = "Title must be at most 150 characters long.")]
        public string title { get; set; }


        public string status { get; set; }
    }
}