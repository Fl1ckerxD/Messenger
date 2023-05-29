using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Messages = new HashSet<Message>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Theme { get; set; } = null!;
        public int? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
