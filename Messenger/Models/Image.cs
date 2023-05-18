using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string ImageName { get; set; } = null!;
        public byte[] ImageData { get; set; } = null!;

        public virtual Message Message { get; set; } = null!;
    }
}
