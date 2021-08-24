using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Dirs = Directory.GetDirectories(root).Select(d => d.Substring(root.Length + 1)).ToList();
        }
    }
}
