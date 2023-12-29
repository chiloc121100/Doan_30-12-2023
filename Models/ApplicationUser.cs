using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuctionProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add any additional properties or methods here
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        public string PhoneNumber { get; set; }
        public int? Money { get; set; }
        // test noti
        public int? NotificationCount { get; set; }
    }
}
