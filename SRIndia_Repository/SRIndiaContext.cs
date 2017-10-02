using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SRIndia_Models.Models;

namespace SRIndia_Repository
{
    public class SRIndiaContext : IdentityDbContext<AppUser>
    {
        public SRIndiaContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Message> Messages { get; set; }
    }
}
