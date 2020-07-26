using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Almwdeh.Models
{
    public class MessageAndAttachments
    {
        public MessagesTbl msg { set; get; }
        public List<MessageAttachmentsTbl> attachments { set; get; }

    }
}