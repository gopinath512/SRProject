using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SRIndia_Repository
{
    public class AppUser : IdentityUser
    {
        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
