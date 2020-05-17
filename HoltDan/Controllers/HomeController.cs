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
            ViewBag.SongsRoot = Server.MapPath("~/media/Songs/");
            //var vm = new PhotosViewModel(Server, photosRoots, "Family");

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
            if (album.StartsWith("bands_"))
                album = album.Replace("bands_", "bands/");

            var dm = new DirMgr(Server.MapPath($"~/media/songs/{album}/"), $"{album}");
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
    }
}
