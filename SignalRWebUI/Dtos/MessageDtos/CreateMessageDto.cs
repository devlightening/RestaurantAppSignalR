namespace SignalRWebUI.Dtos.MessageDtos
{
    public class CreateMessageDto
    {
        public string Content { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
    }
}
