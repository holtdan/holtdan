using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoltDan.Models
{
    public class PhotosPlaylistModel
    {
        //public PhotosViewModel.Intervals Duration { get; set; }
        //public PhotosViewModel.Orders Sequence { get; set; }

        public List<string> DirName { get; set; }

        static public PhotosPlaylistModel CreateFromJson(string jsonStr)
        {
            var ppm = Newtonsoft.Json.JsonConvert.DeserializeObject<PhotosPlaylistModel>(jsonStr);

            return ppm;
        }
        public string AsJson()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return json;
        }
    }
}