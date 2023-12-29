using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AuctionProject.Models
{
    /* public class Product
     {
         [Key]
         public int Id { get; set; }
         public string? Title { get; set; }
         public string? Description { get; set; }
         public bool? IsPublic { get; set; }
         public DateTime? DateStart { get; set; }
         public DateTime? DateEnd { get; set; }
         public string? Image { get; set; }
         public float? PriceStart { get; set; }
         public float? PriceEnd { get; set; }
         public float? PriceJump { get; set; }
         public float? PriceFinish { get; set; }
         public ApplicationUser? User { get; set; }
         [NotNull]
         public ApplicationUser? UserGet { get; set; }
         public ApplicationUser? TestJoin { get; set; }
         public string? ListUserJoin { get; set; }
     }*/
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a title.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Please enter a description.")]
        public string? Description { get; set; }
        public bool? IsPublic { get; set; }
        public bool? isBuyNow { get; set; }
        [Required(ErrorMessage = "Please choose a start date.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime? DateStart { get; set; }
        [Required(ErrorMessage = "Please choose a end date.")]
        public DateTime? DateEnd { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Please enter a start price.")]
        public float? PriceStart { get; set; }
        [Required(ErrorMessage = "Please enter a end price.")]
        public float? PriceEnd { get; set; }
        [Required(ErrorMessage = "Please enter a jump price.")]
        public float? PriceJump { get; set; }
        [Required(ErrorMessage = "Please enter a finish price.")]
        [Display(Name = "Price Bidding")]
        public float? PriceFinish { get; set; }
        public ApplicationUser? User { get; set; }
        public ApplicationUser? UserGet { get; set; }
        public ApplicationUser? TestJoin { get; set; }
        public string? ListUserJoin { get; set; }
        public float? SaveMoney { get; set; }
    }
}
