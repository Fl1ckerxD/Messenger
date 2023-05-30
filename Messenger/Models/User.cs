using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class User
    {
        public User()
        {
            MessageUsers = new HashSet<MessageUser>();
            Messages = new HashSet<Message>();
            Chats = new HashSet<Chat>();
        }

        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public byte[]? Image { get; set; }
        public int? StatusId { get; set; }
        public int? PostId { get; set; }
        public int? DepartmentId { get; set; }
        public int? UserTypeId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual Post? Post { get; set; }
        public virtual Status? Status { get; set; }
        public virtual UserType? UserType { get; set; }
        public virtual ICollection<MessageUser> MessageUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }
    }
}
