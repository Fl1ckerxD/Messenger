using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class Status
    {
        public Status()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
