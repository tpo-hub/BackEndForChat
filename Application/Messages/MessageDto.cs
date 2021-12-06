using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public User.UserDto Sender { get; set; }
        public MessageType MessageType { get; set; }
    }
}
