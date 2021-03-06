﻿using System;
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
        public string Dir { get; private set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public List<FileSet> Files { get; set; }

        static string[] AudioSuffixes = { ".mp3", ".wav" };
        static string[] ImageSuffixes = { ".png", ".jpg", ".bmp", ".gif" };
        static string[] TextSuffixes = { ".html", ".txt" };

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

            if (File.Exists($"{dir}Notes.html"))
                Notes = File.ReadAllText($"{dir}Notes.html");

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
                    var fname = $"{Dir}{Path.DirectorySeparatorChar}{f.FileName}{e}";

                    if (AudioSuffixes.Contains(e))
                    {
                        fs.AudioFileName = $"{refRoot}{Path.DirectorySeparatorChar}{Path.GetFileName(fname)}";

                        TagLib.File taglibFile = TagLib.File.Create(fname, TagLib.ReadStyle.Average);
                        fs.SecondsDuration = (int)taglibFile.Properties.Duration.TotalSeconds;
                        fs.AudioFileInfo = new AudioFileInfo
                        {
                            TrackNum = (int)taglibFile.Tag.Track,
                            Album = taglibFile.Tag.Album,
                            Title = taglibFile.Tag.Title
                        };

                    }
                    else if (TextSuffixes.Contains(e))
                        fs.Notes = File.ReadAllText(fname);
                    else if (ImageSuffixes.Contains(e))
                        fs.ImageFileName = Path.GetFileName(fname);
                }
                if (fs.IsUseful)
                    Files.Add(fs);
            }
            //this.Dirs = dirNames.Select(d => new CheckboxItem { ID = d, Text = d }).ToList();
            //
            // TODO: Add constructor logic here
            //
        }
#if false
        public static IEnumerable<string> GetDirNames(HttpServerUtilityBase server, string dir)
        {
            //var dirs = Directory.GetDirectories(dir);
            var dirs = Directory.GetDirectories(server.MapPath(dir));
            return dirs;
        }
        public static IEnumerable<string> GetFileNames(string dir, string mask)
        {
            var files = Directory.GetFiles(dir, mask,SearchOption.AllDirectories);

            //var x = files.Select(f => Path.GetDirectoryName(f)).ToList();
            //var y = files.Select(f => Path.GetExtension(f)).ToList();
            //var z = files.Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
            //var a = files.Select(f => Path.GetFullPath(f)).ToList();
            var b = files.Select(f => Path.GetFullPath(f)).ToList();
            return b; // files.Select(f => Path.GetFileNameWithoutExtension(f)).AsEnumerable();
        }
        public static IEnumerable<_File> GetImgFiles(string realDir, params string[] masks)
        {
            var files = new List<string>();

            var relDir = realDir.Substring(realDir.LastIndexOf("\\media\\"));

            foreach (var m in masks)
            {
                files.AddRange(Directory.GetFiles(realDir, m));
            }

            //var x = files.Select(f => Path.GetDirectoryName(f)).ToList();
            //var y = files.Select(f => Path.GetExtension(f)).ToList();
            //var z = files.Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
            //var a = files.Select(f => Path.GetFullPath(f)).ToList();
            var b = files.OrderBy(s => s).Select(f => new _File { NameOnly = Path.GetFileNameWithoutExtension(f), ImgSrc = relDir + Path.GetFileName(f) }).ToList();
            return b; // files.Select(f => Path.GetFileNameWithoutExtension(f)).AsEnumerable();
        }
        public static string ImgDiv(string name, bool showFleNames, string src, string imgDivClass)
        {
            //var a = Path.
            var sb = new StringBuilder(
                showFleNames ?
                    string.Format("<div class='{0}'>{1}<br /><img src='{2}'></div>", imgDivClass, name, src) :
                    string.Format("<div class='{0}'><img src='{2}'></div>", imgDivClass, name, src));

            return sb.ToString();
        }
        public static HtmlString ImgSet(string imgSetClass, string imgDivClass, string name, bool showFleNames, string dir, params string[] masks)
        {
            if (!dir.EndsWith("\\")) dir += "\\";

            var sb = new StringBuilder(string.Format("<div class='{0}'><h2>{1}</h2>", imgSetClass, name));

            foreach (var file in DirMgr.GetImgFiles(dir, masks))
            {
                sb.AppendLine(ImgDiv(file.NameOnly, showFleNames, file.ImgSrc, imgDivClass));
            }
            sb.AppendLine("</div>");

            return new HtmlString(sb.ToString());
        }
#endif
    }
}
