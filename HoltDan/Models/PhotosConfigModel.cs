using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace HoltDan.Models
{
    public class PhotosConfigModel
    {
        public class Gallery
        {
            public string ID { get; set; }
            public string Text { get; set; }
            public string Root { get; set; }
            public bool Listed { get; set; }
        }
        public class Interval
        {
            public int NumSeconds { get; set; }
            public string Text { get; set; }
        }
        public IEnumerable<Gallery> Galleries { get; set; }
        public IEnumerable<Interval> Intervals { get; set; }

        public PhotosConfigModel(string configFileName)
        {
            using (XmlReader xr = XmlReader.Create(configFileName))
            {
                var xdoc = XDocument.Load(xr);

                this.Galleries = (from t in xdoc.Descendants("gallery")
                                  select new Gallery
                                  {
                                      ID = t.Attribute("id").Value.ToLower(),
                                      Text = t.Element("text").Value,
                                      Root = t.Element("root").Value,
                                      Listed = bool.Parse(t.Element("listed").Value)
                                  }
                             ).ToList();

                this.Intervals = (from t in xdoc.Descendants("interval")
                                  select new Interval
                                  {
                                      NumSeconds = (int)TimeSpan.Parse(t.Element("timespan").Value).TotalSeconds,
                                      Text = t.Element("text").Value
                                  }
                             ).ToList();
            }
        }
    }
}