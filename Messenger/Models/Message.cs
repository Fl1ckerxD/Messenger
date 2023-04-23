using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class Message
    {
        public Message()
        {
            MessageUsers = new HashSet<MessageUser>();
        }

        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Time { get; set; }

        public virtual Chat Chat { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<MessageUser> MessageUsers { get; set; }
    }
}
