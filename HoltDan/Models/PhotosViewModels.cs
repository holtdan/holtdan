using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Models
{
    public class PhotosViewModel
    {
        public SelectList IntervalSelList { get; set; }

        [Display(Name = "Folder")]
        public string Dir { get; set; }
        [Display(Name = "Interval")]
        public int IntervalSeconds { get; set; }
        [Display(Name = "Random Sequence")]
        public bool RandomSequence { get; set; }

        public List<CheckboxItem> Dirs { get; set; }
        public List<CheckboxItem> Playlists { get; set; }

        public IEnumerable<string> DirNames { get; set; }
        public IEnumerable<string> PlaylistNames { get; set; }

        public PhotosViewModel()
        {
            IntervalSeconds = 10;
            RandomSequence = true;
        }
        public PhotosViewModel(HttpServerUtilityBase server, string dir)
            :this()
        {
            this.Dir = dir;
            var realDir = server.MapPath(dir);

            var dirs = Directory.GetDirectories(realDir);
            this.DirNames = dirs.Select(d => d.Substring(d.LastIndexOf("\\") + 1)).ToList();
            this.Dirs = DirNames.Select(d => new CheckboxItem { ID = d, Text = d }).ToList();

            var idx = Directory.GetFiles(realDir, "*.txt");

            this.PlaylistNames = idx.Select(s => s.Substring(s.LastIndexOf("\\") + 1));
            this.Playlists = PlaylistNames.Select(d => new CheckboxItem
            { 
                ID = d,
                Text = d.Substring(0, d.Length - 4) // remove ".txt"
            }).ToList();
        }
    }
}
