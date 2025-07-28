namespace SignalR.EntityLayer.Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Foreign Keys
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }

        // Navigation Properties
        public AppUser SenderUser { get; set; }
        public AppUser ReceiverUser { get; set; }
    }
}
