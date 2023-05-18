using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileExtension { get; set; } = null!;
        public byte[] FileData { get; set; } = null!;
        public long FileLength { get; set; }

        public virtual Message Message { get; set; } = null!;
    }
}
