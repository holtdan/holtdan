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
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public SongViewModel() { }
        public SongViewModel(string fileName, string title, string notes)
        {
            FileName = fileName;
            Title = title;
            Notes = notes;
        }
    }
}
