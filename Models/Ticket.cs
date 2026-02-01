using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Helpdesk_System.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Creator (required)
        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        // AssignedTo (optional)
        public string? AssignedToId { get; set; } 
        public ApplicationUser? AssignedTo { get; set; }
    }
}
