using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class Post
    {
        public Post()
        {
            Users = new HashSet<User>();
            Departments = new HashSet<Department>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
