using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoltDan.Models
{
    public class SongAlbumViewModel
    {
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Songs")]
        public List<SongViewModel> Songs { get; set; }

        public SongAlbumViewModel() { }
        public SongAlbumViewModel(string title, IEnumerable<SongViewModel> songs,  string notes)
        {
            Title = title;
            Notes = notes;
            Songs = new List<SongViewModel>(songs);
        }
    }
    public class SongViewModel
    {
        [Display(Name = "Track Number")]
        public int TrackNum { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public int SecondsDuration { get; set; }
        public string DurationFormatted() => $"{(int)(SecondsDuration/60)}:{(SecondsDuration%60).ToString("00")}";

        public SongViewModel() { }
        public SongViewModel(int trackNum, string fileName, string title, string notes, int secondsDurarion)
        {
            TrackNum = trackNum;
            FileName = fileName;
            Title = title;
            Notes = notes;
            SecondsDuration = secondsDurarion;
        }
    }
}
