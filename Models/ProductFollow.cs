using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AuctionProject.Models
{
    public class ProductFollow
    {
        [Key]
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string? UserId { get; set; }
        public bool ? IsFollow { get; set; }
    }
}
