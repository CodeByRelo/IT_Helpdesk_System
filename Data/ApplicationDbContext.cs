using IT_Helpdesk_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IT_Helpdesk_System.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TicketAssignment> TicketAssignments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
    }
}
