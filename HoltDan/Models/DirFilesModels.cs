using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HoltDan.Models
{
    public class DirFilesModels
    {
    }
    public class DirMgr
    {
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

        public class _File
        {
            public string NameOnly { get; set; }
            public string ImgSrc { get; set; }
        }
        public DirMgr()
        {
            //
            // TODO: Add constructor logic here
            //
        }
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

            var relDir = realDir.Substring(realDir.LastIndexOf("\\images\\"));

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
    }
}