using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HoltDan.Models
{
    public class PhotosViewModel
    {
        public enum Intervals
        {
            Secs3,
            Secs10,
            Secs30,
            Min1,
            Min5,
            Min10,
        }
        public enum Orders
        {
            Random,
            Sequential
        }
        public Intervals Interval { get; set; }
        public Orders Order { get; set; }
        public List<CheckboxItem> Dirs { get; set; }

        public int IntervalAsMiliseconds
        {
            get
            {
                switch (this.Interval)
                {
                    case Intervals.Secs3:
                        return 3000;
                    case Intervals.Secs10:
                        return 10000;
                    case Intervals.Secs30:
                        return 30000;
                    case Intervals.Min1:
                        return 60000;
                    case Intervals.Min5:
                        return 60000 * 5;
                    case Intervals.Min10:
                        return 60000 * 10;
                    default:
                        return 5000;
                }
            }
        }
        public PhotosViewModel()
        {
        }
        public PhotosViewModel(HttpServerUtilityBase server, string dir)
        {
            var dirs = Directory.GetDirectories(server.MapPath(dir));

            this.Dirs = dirs.Select(d => new CheckboxItem { ID = d, Text = d.Substring(d.LastIndexOf("\\") + 1) }).ToList();
        }
    }
}