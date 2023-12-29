using AuctionProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuctionProject.Models.Product> Product { get; set; } = default!;
        public DbSet<AuctionProject.Models.ApplicationUser> User { get; set; } = default!;
        public DbSet<AuctionProject.Models.UsersPage> UsersPage { get; set; } = default!;
        public DbSet<AuctionProject.Models.ProductFollow> ProductFollow { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }

    }
}