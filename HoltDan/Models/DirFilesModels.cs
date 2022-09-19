using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HoltDan.Models
{
    public static class DirFiles
    {
        public static string PastLast(this string path, string lastWhat)
        {
            return path.Substring(path.LastIndexOf(lastWhat) + 1);
        }
        public static string DirLeaf(this string path)
        {
            var sep = Path.DirectorySeparatorChar;
            var p1 = path.TrimEnd(sep);
            var p2 = p1.Substring(p1.LastIndexOf(sep) + 1);
            return p2;
        }
    }
    public class DirMgr
    {
        //public string RefRoot { get; private set; } 
        public string Dir { get; private set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public List<FileSet> Files { get; set; }

        static public string[] AudioSuffixes = { ".mp3", ".wav" };
        static public string[] ImageSuffixes = { ".png", ".jpg", ".bmp", ".gif" };
        static public string[] TextSuffixes = { ".html", ".txt" };

        static Random _random = new Random();
        static public void Shuffle<T>(T[] array)
            {
                int n = array.Length;
                for (int i = 0; i < n; i++)
                {
                    // NextDouble returns a random number between 0 and 1.
                    // ... It is equivalent to Math.random() in Java.
                    int r = i + (int)(_random.NextDouble() * (n - i));
                    T t = array[r];
                    array[r] = array[i];
                    array[i] = t;
                }
            }

        public class AudioFileInfo
        {
            public int TrackNum { get; set; }
            public string Title { get; set; }
            public string Album { get; set; }
        }
        public class FileSet
        {
            public string Title { get; set; }
            public string AudioFileName { get; set; }
            public int SecondsDuration { get; set; }
            public string ImageFileName { get; set; }
            // VideoFileName, ?
            public string Notes { get; set; }
            //public string GetAudioFile() => FileNames.SingleOrDefault(fn => AudioSuffixes.Contains(fn));\
            public bool IsUseful { get { return AudioFileName != null || ImageFileName != null; } }

            public AudioFileInfo AudioFileInfo { get; set; }
        }
        public DirMgr(string dir, string refRoot)
        {
            this.Dir = dir;
            this.Title = Dir.DirLeaf();
            var notesFn = dir + Title + ".html";
            if (File.Exists(notesFn))
                Notes = File.ReadAllText(notesFn);

            var files = (from f in Directory.GetFiles(Dir).ToArray()
                         let fName = Path.GetFileNameWithoutExtension(f)
                         let fExt = Path.GetExtension(f.ToLower())
                         group f by fName into g            //var dirNames = Directory.GetDirectories(dir).Select(d => d.Substring(d.LastIndexOf("\\") + 1)).ToList();
                         select new { FileName = g.Key, Exts = g.Select(f => Path.GetExtension(f)).ToList() }// g.Select(f=>f.FileExt).To}
                        ).ToList();

            Files = new List<FileSet>();
            foreach (var f in files)
            {
                var fs = new FileSet() { Title = f.FileName };
                foreach (var e in f.Exts)
                {
                    var fname = $"{refRoot}{f.FileName}{e}";
                    var fileSysName = $"{dir}{f.FileName}{e}";

                    if (AudioSuffixes.Contains(e))
                    {
                        fs.AudioFileName = fname;// $"{refRoot}{Path.DirectorySeparatorChar}{Path.GetFileName(fname)}";
                        TagLib.File taglibFile = TagLib.File.Create(fileSysName, TagLib.ReadStyle.Average);
                        fs.SecondsDuration = (int)taglibFile.Properties.Duration.TotalSeconds;
                        fs.AudioFileInfo = new AudioFileInfo
                        {
                            TrackNum = (int)taglibFile.Tag.Track,
                            Album = taglibFile.Tag.Album,
                            Title = taglibFile.Tag.Title
                        };
                    }
                    else if (TextSuffixes.Contains(e))
                        fs.Notes = File.ReadAllText(fileSysName);
                    else if (ImageSuffixes.Contains(e))
                        fs.ImageFileName = Path.GetFileName(fname);
                }
                if (fs.IsUseful)
                    Files.Add(fs);
            }
        }
    }
}
