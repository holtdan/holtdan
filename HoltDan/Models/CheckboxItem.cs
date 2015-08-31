using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HoltDan.Models
{
    [DebuggerDisplay("{ID}.{Text} {IsSelected}")]
    public class CheckboxItem
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }
}
