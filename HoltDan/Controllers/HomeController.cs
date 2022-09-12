using HoltDan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Controllers
{
    public class HomeController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var photosPath = Server.MapPath("~/media/Photos");
            var photosRoots = Directory.GetDirectories(photosPath).Select(d => d.Substring(d.LastIndexOf("\\") + 1)).ToList();
            ViewBag.PhotoRoots = photosRoots;
            ViewBag.MediaRoot = Server.MapPath("~/media/");
            ViewBag.MusicRoot = Server.MapPath("~/media/Music");
            ViewBag.SongsRoot = Server.MapPath("~/media/Songs/");
            var songsRoots = Directory.GetDirectories((string)ViewBag.SongsRoot);//.Select(d => d.Substring(d.LastIndexOf("\\") + 1)).ToList();
            ViewBag.SongRoots = songsRoots;
            //var vm = new PhotosViewModel(Server, photosRoots, "Family");

            ViewBag.SongsMap = (from d in songsRoots
                                orderby d
                                select new
                                {
                                    dir = d.Substring(d.LastIndexOf("\\") + 1),
                                    dirs = Directory.GetDirectories(d).Select(a => a.Substring(a.LastIndexOf("\\") + 1)).ToList()
                                }).ToDictionary(k => k.dir, v => v.dirs);

            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index()
        {
            //DropboxRestAPI.Client client = new DropboxRestAPI.Client(new DropboxRestAPI.Options
            //{
            //    AccessToken = "vBgoZGNyPIMAAAAAAAA0GTo_t9tHdiE_f4EMOGNoelpkTKu7-bWzVNxAN9rdeM4E"
            //});
            //var what = await client.Core.Metadata.MetadataAsync("/Art", include_media_info: true);

            //foreach (var c in what.contents)
            //{
            //    var x = await client.Core.Metadata.SharesAsync(c.path);
            //}
            //var token = client.Core.OAuth2.TokenAsync(authCode);
            return View();
        }
        public ActionResult Photos(string id)
        {
            var photosPath = Server.MapPath($"~/media/Photos/{id}");
            var vm = new PhotosViewModel(photosPath);
            return View(vm);
        }
        [HttpPost]
        public ActionResult ShowPhotos(PhotosViewModel vm)
        {
            var dirs = vm.Dirs.Where(d => d.IsSelected).Select(d => d.ID).ToList();

            if (dirs.Count() == 0)
                return RedirectToAction("Index");

            ViewBag.HoldSeconds = (int)TimeSpan.Parse(vm.IntervalSpan).TotalSeconds;
            return View(vm.BuildShow(Server.MapPath($"~/media/Photos/")));
        }
        public ActionResult Guitars()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Songs(string album)
        {
            var dm = new DirMgr(Server.MapPath($"~/media/songs/{album}/"), $"/media/songs/{album}/");
            return View("SongAlbum",dm);
        }
        public ActionResult Hands()
        {
            return View();
        }
        public ActionResult Zen()
        {
            return View();
        }
        public ActionResult Scales()
        {
            return View("Scales");
        }
        public ActionResult Frets()
        {
            return View();
        }
        public ActionResult Music()
        {
            return View(new MusicViewModel(ViewBag.MusicRoot));
        }
        public PartialViewResult AlbumList(string artist)
        {
            var path = $"{ViewBag.MusicRoot}\\{artist}";
            return PartialView("_AlbumList",
                (from d in Directory.GetDirectories(path)
                 orderby d
                 select d.Substring(path.Length + 1)
                 ).ToList()
                );
        }
        //[Route("music/{artist}/{album}")]
        public PartialViewResult Album(string artist, string album)
        {
            var refRoot = $"/media/music/{artist}/{album}/";

            var dm = new DirMgr(Server.MapPath(refRoot), refRoot);
            return PartialView("_Album", dm);
        }
        public ActionResult Play(string artist, string album)
        {
            var songRoots = Directory.GetDirectories((string)ViewBag.SongsRoot).Select(s => s.DirLeaf().ToLower());
            var isDan = songRoots.Contains(artist.ToLower());
            var refRoot = isDan ? $"/media/songs/{artist}/{album}/" : $"/media/music/{artist}/{album}/";

            var dm = new DirMgr(Server.MapPath(refRoot), refRoot);

            ViewBag.MinimalLayout = true;
            return View("SongAlbum", dm);
        }
    }
}
