namespace AuctionProject.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int Idproduct { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
    }
}
