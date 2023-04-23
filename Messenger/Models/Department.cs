using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
