//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Almwdeh.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MessageAttachmentsTbl
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public Nullable<int> MessageIDs { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
    
        public virtual MessagesTbl MessagesTbl { get; set; }
    }
}
