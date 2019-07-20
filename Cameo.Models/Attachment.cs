using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cameo.Models
{
    public class Attachment : BaseModel
    {
        [Required]
        [StringLength(32)]
        public string GUID { get; set; }

        [Required]
        [StringLength(255)]
        public string Filename { get; set; }

        [Required]
        [StringLength(512)]
        public string Path { get; set; }

        [Required]
        [StringLength(32)]
        public string Extension { get; set; }

        public long Size { get; set; } //byte

        [StringLength(128)]
        public string MimeType { get; set; }
    }
}
