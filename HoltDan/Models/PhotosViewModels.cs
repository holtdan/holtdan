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

        [Display(Name = "Interval")]
        public int IntervalSeconds { get; set; }
        [Display(Name = "Random Sequence")]
        public bool RandomSequence { get; set; }

        public List<CheckboxItem> Dirs { get; set; }
        public List<CheckboxItem> Playlists { get; set; }

        public PhotosViewModel()
        {
            IntervalSeconds = 10;
            RandomSequence = true;
        }
        public PhotosViewModel(HttpServerUtilityBase server, string dir)
            :this()
        {
            var realDir = server.MapPath(dir);

            var dirs = Directory.GetDirectories(realDir);

            this.Dirs = dirs.Select(d => new CheckboxItem { ID = d, Text = d.Substring(d.LastIndexOf("\\") + 1) }).ToList();

            var idx = Directory.GetFiles(realDir, "*.txt");

            this.Playlists = idx.Select(d => new CheckboxItem 
            { 
                ID = d,
                Text = d.Substring(0, d.Length - 4).Substring(d.LastIndexOf("\\") + 1) // skip path, remove ".txt"
            }).ToList();
        }
    }
}
