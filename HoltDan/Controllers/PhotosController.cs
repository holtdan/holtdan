using HoltDan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace HoltDan.Controllers
{
    public class PhotosController : Controller
    {
        // GET: Photos
        public ActionResult Index(string id)
        {
            ViewBag.Title = "Family";
            var dirName = "~/media/Family"; // the default


            var fPath = Server.MapPath("~/App_Data/Photos.config");

            using (XmlReader xr = XmlReader.Create(fPath))
            {
                var xdoc = XDocument.Load(xr);

                var gals = (from t in xdoc.Descendants("gallery")
                            select new
                            {
                                ID = t.Attribute("id").Value.ToLower(),
                                Text = t.Element("text").Value,
                                Root = t.Element("root").Value
                            }
                             ).ToList();

                if (id != null)
                {
                    var play = gals.Where(q => q.ID == id.ToLower()).SingleOrDefault();

                    if (play != null)
                    {
                        ViewBag.Title = play.Text;
                        dirName = "~/media/" + play.Root;
                    }
                }
                var vm = new PhotosViewModel(Server, dirName);
                vm.IntervalSelList = new SelectList((from t in xdoc.Descendants("interval")
                                                     select new
                                                     {
                                                         ID = (int)TimeSpan.Parse(t.Element("timespan").Value).TotalSeconds,
                                                         Text = t.Element("text").Value
                                                     }
                             ).ToList(), "ID", "Text", vm.IntervalSeconds);

                return View(vm);
            }
            //var ppm = new PhotosPlaylistModel();

            //ppm.Duration = PhotosViewModel.Intervals.Secs30;
            //ppm.Sequence = PhotosViewModel.Orders.Sequential;

            //ppm.DirName = new List<string> { "A.png", "B.jpg", "C.gif" };
            //var s = ppm.AsJson();
            //var ppm2 = PhotosPlaylistModel.CreateFromJson(s);

        }
        [HttpPost]
        public ActionResult Index(PhotosViewModel vm)
        {
            var dirs = vm.Dirs.Where(d => d.IsSelected).Select(d => d.ID).ToList();

            if (vm.Playlists != null) // none defined
            {
                var pres = vm.Playlists.Where(d => d.IsSelected).Select(d => d.ID).ToList();

                foreach (var p in pres)
                {
                    var fname = p;
                    var lines = System.IO.File.ReadAllLines(fname);

                    foreach (var l in lines)
                    {
                        //var a = Server.MapPath(l);
                        //var b = Server.UrlPathEncode(l);
                        //var c = Server.UrlPathEncode(a);
                        dirs.Add(Server.MapPath(l));
                    }
                }
            }

            if (dirs.Count() == 0)
                return RedirectToAction("Index");

            var files = new List<string>();

            foreach (var d in dirs)
            {
                files.AddRange(Directory.GetFiles(d, "*.*", SearchOption.AllDirectories).Select(s => s.Substring(s.LastIndexOf("\\media\\")).Replace("\\", "/")));
            }

            var fileArr = files.ToArray();

            ViewBag.IntervalAsMiliseconds = vm.IntervalSeconds * 1000;
            ViewBag.HoldSeconds = vm.IntervalSeconds;

            if (vm.RandomSequence)//.Order == PhotosViewModel.Orders.Random)
            {
                DirMgr.Shuffle(fileArr);
            }

            return View("Photos", fileArr);
        }
    }
}
