using HoltDan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace HoltDan.Controllers
{
#if false
    public class PhotosController : Controller
    {

        // GET: Photos
        public ActionResult Index(string id)
        {
            var dbgStr = new StringBuilder();

            //var cookie = Request.Cookies["selStudios"];
            var photosPath = Server.MapPath("~/media/Photos");
            var photosRoots = Directory.GetDirectories(photosPath);

            var vm = new PhotosViewModel(Server, photosRoots, "Family");
            ViewBag.IntervalAsMiliseconds = vm.IntervalSeconds * 1000;
            ViewBag.HoldSeconds = vm.IntervalSeconds;
#if false
            if (id != null && !isGallery)
            {
                var isPlaylist = vm.PlaylistNames.Select(s => s.ToLower()).Contains(id.ToLower()+".txt");
                var isDir = vm.DirNames.Select(s => s.ToLower()).Contains(id.ToLower());
                var allFiles = new List<string>();

                if (isPlaylist)
                {
                    var dirs = ExpandPlaylists(dirName, new[] { id + ".txt" }); //Enumerable.Repeat(item, 1);

                    allFiles.AddRange(ExtractDirsFiles(dirName, dirs));
                }
                else if (isDir)
                {
                    allFiles.AddRange(ExtractDirsFiles(dirName, new[] { id }));
                }

                if (allFiles.Count > 0) // otherwise just fall through defaults below
                {
                    var mapDir = Server.MapPath(dirName);

                    return View("Photos", allFiles.Select(f=> dirName + f).Select(f=>f.Replace("\\","/")));
                }
            }
#endif
            //vm.IntervalSelList = new SelectList(
            //    pcm.Intervals, "NumSeconds", "Text", vm.IntervalSeconds);

            ViewBag.DbgStr = dbgStr.ToString();
            return View(vm);
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
            //HttpCookie stdsCook = new HttpCookie("selStudios", selStr)
            //{
            //    Expires = DateTime.Now.AddDays(5)
            //};
            //Response.AppendCookie(stdsCook);

            var dirs = vm.Dirs.Where(d => d.IsSelected).Select(d => d.ID).ToList();

            if (vm.Playlists != null) // none defined
            {
                var pres = ExpandPlaylists(vm.Dir, vm.Playlists.Where(d => d.IsSelected).Select(d => d.ID));

                dirs.AddRange(pres);
            }

            if (dirs.Count() == 0)
                return RedirectToAction("Index");

            var fileArr = ExtractDirsFiles(vm.Dir, dirs, vm.RandomSequence);

            ViewBag.IntervalAsMiliseconds = vm.IntervalSeconds * 1000;
            ViewBag.HoldSeconds = vm.IntervalSeconds;
            ViewBag.Dir = vm.Dir;

            return View("Photos", fileArr.Select(f => vm.Dir + f).Select(f => f.Replace("\\", "/")));
        }

        IEnumerable<string> ExpandPlaylists(string root, IEnumerable<string> pListName)
        {
            var dirs = new List<string>();

            foreach (var p in pListName)
            {
                var ln = root + p;// p.Replace("\\", "/");
                //ln = ln.Substring(ln.LastIndexOf("/media/"));

                var fname = Server.MapPath(ln);
                var lines = System.IO.File.ReadAllLines(fname);

                foreach (var l in lines)
                {
                    //var ff = l.Replace("\\", "/");
                    //ff = ff.Substring(ff.LastIndexOf("/media/"));

                    var lName = l;// Server.MapPath(root + l);
                    //var a = Server.MapPath(l);
                    //var b = Server.UrlPathEncode(l);
                    //var c = Server.UrlPathEncode(a);
                    dirs.Add(lName);
                }
            }

            return dirs;
        }

        string[] ExtractDirsFiles(string root, IEnumerable<string> dirs, bool shuffle = true)
        {
            var files = new List<string>();

            foreach (var d in dirs)
            {
                var dName = Server.MapPath(root + d);
                if (Directory.Exists(dName))
                {
                    var foundFiles = Directory.GetFiles(dName, "*.*", SearchOption.AllDirectories).Select(s => s.Replace("\\", "/"));
                    files.AddRange(foundFiles.Select(s => d + "/" + s.Substring(s.LastIndexOf("/")+1)));
                }
            }

            var fileArr = files.ToArray();

            if (shuffle)//.Order == PhotosViewModel.Orders.Random)
            {
                DirMgr.Shuffle(fileArr);
            }

            return fileArr;
        }
    }
#endif
}
