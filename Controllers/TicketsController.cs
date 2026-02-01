using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IT_Helpdesk_System.Data;
using IT_Helpdesk_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace IT_Helpdesk_System.Controllers
{
    [Authorize] // Require login for ALL ticket actions
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ======================== ACTIVE TICKETS ========================

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            IQueryable<Ticket> query = _context.Tickets
                .Where(t => t.Status != "Closed")
                .Include(t => t.AssignedTo);

            if (!isAdmin)
            {
                query = query.Where(t => t.CreatedById == user.Id);
            }

            var tickets = await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(tickets);
        }

        // ======================== CLOSED TICKETS ========================

        // GET: Tickets/History
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            IQueryable<Ticket> query = _context.Tickets
                .Where(t => t.Status == "Closed")
                .Include(t => t.AssignedTo);

            if (!isAdmin)
            {
                query = query.Where(t => t.CreatedById == user.Id);
            }

            var closedTickets = await query
                .OrderByDescending(t => t.UpdatedAt)
                .ToListAsync();

            return View(closedTickets);
        }

        // ======================== DETAILS ========================

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!isAdmin && ticket.CreatedById != user.Id)
                return Forbid();

            return View(ticket);
        }

        // ======================== CREATE ========================

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Ticket ticket)
        {
            if (!ModelState.IsValid)
                return View(ticket);

            var user = await _userManager.GetUserAsync(User);

            ticket.CreatedAt = DateTime.Now;
            ticket.Status = "Open";
            ticket.CreatedById = user.Id;

            _context.Add(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================== EDIT ========================

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!isAdmin && ticket.CreatedById != user.Id)
                return Forbid();

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(ticket);

            var existingTicket = await _context.Tickets.FindAsync(id);
            if (existingTicket == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!isAdmin && existingTicket.CreatedById != user.Id)
                return Forbid();

            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.Status = ticket.Status;
            existingTicket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ======================== CLOSE TICKET ========================

        // POST: Tickets/Close/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);

            if (!isAdmin && ticket.CreatedById != user.Id)
                return Forbid();

            ticket.Status = "Closed";
            ticket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(History));
        }

        // ======================== DELETE (ADMIN ONLY) ========================

        // GET: Tickets/Delete/5
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null)
                _context.Tickets.Remove(ticket);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
