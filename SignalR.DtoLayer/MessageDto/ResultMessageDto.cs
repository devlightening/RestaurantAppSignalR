using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DtoLayer.MessageDto
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
