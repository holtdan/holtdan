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
        public string Dir { get; set; }

        [Display(Name = "Interval")]
        public string IntervalSpan { get; set; }
        [Display(Name = "Random Sequence")]
        public bool RandomSequence { get; set; }

        public List<CheckboxItem> Dirs { get; set; }

        public PhotosViewModel()
        {
            IntervalSpan = "0:0:5";
        }
        public PhotosViewModel(string dir)
            :this()
        {
            this.Dir = dir.Substring(dir.LastIndexOf("\\") + 1);
            var dirNames = Directory.GetDirectories(dir).Select(d => d.Substring(d.LastIndexOf("\\") + 1)).ToList();
            this.Dirs = dirNames.Select(d => new CheckboxItem { ID = d, Text = d }).ToList();

            //var idx = Directory.GetFiles(realDir, "*.txt");

            //this.PlaylistNames = idx.Select(s => s.Substring(s.LastIndexOf("\\") + 1));
            //this.Playlists = PlaylistNames.Select(d => new CheckboxItem
            //{ 
            //    ID = d,
            //    Text = d.Substring(0, d.Length - 4) // remove ".txt"
            //}).ToList();
        }
        public IEnumerable<string> BuildShow(string rootDir )
        {
            var files = new List<string>();

            foreach (var d in Dirs.Where(d => d.IsSelected))
            {
                var dName = $"{rootDir}/{Dir}/{d.ID}";
                if (Directory.Exists(dName))
                {
                    var foundFiles = (from f in Directory.GetFiles(dName, "*.*", SearchOption.TopDirectoryOnly)
                                      where DirMgr.ImageSuffixes.Contains(Path.GetExtension(f).ToLower())
                                      orderby f
                                      select f.Replace("\\", "/"));

                    files.AddRange(foundFiles.Select(s => $"~/media/Photos/{Dir}/{d.ID}/{s.Substring(s.LastIndexOf("/") + 1)}"));
                }
            }
            var fileArr = files.ToArray();
            if (RandomSequence)//.Order == PhotosViewModel.Orders.Random)
            {
                DirMgr.Shuffle(fileArr);
            }

            return fileArr;
        }
        string[] ExtractDirsFiles(string root, IEnumerable<string> dirs, bool shuffle = true)
        {
            var files = new List<string>();

            //foreach (var d in dirs)
            //{
            //    var dName = Server.MapPath(root + d);
            //    if (Directory.Exists(dName))
            //    {
            //        var foundFiles = Directory.GetFiles(dName, "*.*", SearchOption.AllDirectories).Select(s => s.Replace("\\", "/"));
            //        files.AddRange(foundFiles.Select(s => d + "/" + s.Substring(s.LastIndexOf("/") + 1)));
            //    }
            //}

            //var fileArr = files.ToArray();

            //if (shuffle)//.Order == PhotosViewModel.Orders.Random)
            //{
            //    DirMgr.Shuffle(fileArr);
            //}

            //return fileArr;
            return files.ToArray();
        }
    }
}
