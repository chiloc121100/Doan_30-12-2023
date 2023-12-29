using System.ComponentModel.DataAnnotations;

namespace AuctionProject.Models
{
    public class Donate
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Money { get; set; }
    }
}
