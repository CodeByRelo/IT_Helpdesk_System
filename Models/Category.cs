using System.ComponentModel.DataAnnotations;

namespace IT_Helpdesk_System.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
