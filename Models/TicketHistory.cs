using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Helpdesk_System.Models
{
    public class TicketHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

        [Required]
        public int UpdatedByUserId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
