using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HoltDan.Models
{
    public class SongsViewModel
    {
        public string Dir { get; set; }

        public List<string> Dirs { get; set; }

        public SongsViewModel()
        {
        }
        public SongsViewModel(string dir)
            : this()
        {
            this.Dir = dir.Substring(dir.LastIndexOf("\\") + 1);
            this.Dirs = Directory.GetDirectories(dir).Select(d => d.Substring(d.LastIndexOf("\\") + 1)).ToList();
        }
    }
}
