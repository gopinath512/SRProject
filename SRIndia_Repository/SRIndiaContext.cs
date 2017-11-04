using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace SRIndia_Repository
{
    public class SrIndiaContext : IdentityDbContext<AppUser>
    {
        public SrIndiaContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageImages> MessageImages { get; set; }

        public DbSet<MessageReply> MessageReply { get; set; }
    }
}
