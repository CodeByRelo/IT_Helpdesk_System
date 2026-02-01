using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IT_Helpdesk_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
    }
}
