using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DtoLayer.MessageDto
{
    public class CreateMessageDto
    {
        public string Content { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
    }
}
