using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; } = null!;
        public string? FileUrl { get; set; }

        public virtual Message Message { get; set; } = null!;
    }
}
