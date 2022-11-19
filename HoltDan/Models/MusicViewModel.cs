using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace HoltDan.Models
{
    public class MusicViewModel
    {
        public string Root { get; set; }
        public List<string> Dirs { get; set; }
        public MusicViewModel(string root)
        {
            Root = root;
            Dirs = (from s in Directory.GetDirectories(root)
                    orderby s.StartsWith("The ", StringComparison.OrdinalIgnoreCase) ?
                            s.Substring(s.IndexOf(" ") + 1) : s
                    select s).ToList();
        }
    }
}
