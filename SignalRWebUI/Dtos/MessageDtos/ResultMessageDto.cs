namespace SignalRWebUI.Dtos.MessageDtos
{
    public class ResultMessageDto
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public string SenderFullName { get; set; }
        public string ReceiverFullName { get; set; }
    }
}
