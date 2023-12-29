using Microsoft.AspNetCore.Identity;

namespace AuctionProject.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Descriptions { get; set; }
    }
}
