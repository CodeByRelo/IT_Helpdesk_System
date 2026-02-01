using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Helpdesk_System.Models
{
    public class TicketAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        public int TechnicianId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
