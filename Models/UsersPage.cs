using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AuctionProject.Models
{
    public class UsersPage
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
