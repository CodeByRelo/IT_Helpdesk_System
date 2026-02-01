using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IT_Helpdesk_System.Data;
using IT_Helpdesk_System.Models;

namespace IT_Helpdesk_System.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ====================== ADMIN DASHBOARD ======================

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(tickets);
        }

        // ====================== TICKET DETAILS ======================

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // ====================== ASSIGN TICKET ======================

        public async Task<IActionResult> Assign(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
                return NotFound();

            var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin);

            ViewBag.Admins = admins;

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, string assignedToId)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            ticket.AssignedToId = assignedToId;
            ticket.Status = "In Progress";
            ticket.UpdatedAt = System.DateTime.Now;

            _context.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ====================== USERS ======================

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
    }
}
