using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FullName { get; set; }
        public string? ProfilePicture { get; set; } = "/uploads/default_profile.jpg";  


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
