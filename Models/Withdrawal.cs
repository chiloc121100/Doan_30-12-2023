namespace AuctionProject.Models
{
    public class Withdrawal
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public WithdrawalStatus Status { get; set; }
        // Other properties as needed
    }

    public enum WithdrawalStatus
    {
        Pending,
        Approved,
        Rejected
    }

}
