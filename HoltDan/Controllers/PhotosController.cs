using HoltDan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Controllers
{
    public class PhotosController : Controller
    {
        // GET: Photos
        public ActionResult Index()
        {
            var dirName = "~/media/Family";

            var vm = new PhotosViewModel(Server, dirName);

            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(PhotosViewModel vm)
        {
            var dirs = vm.Dirs.Where(d => d.IsSelected).Select(d => d.ID).ToList();
            var files = new List<string>();

            foreach (var d in dirs)
            {
                files.AddRange(Directory.GetFiles(d, "*.*" ,SearchOption.AllDirectories).Select(s=>s.Substring(s.LastIndexOf("\\media\\")).Replace("\\","/")) );
            }

            var fileArr = files.ToArray();

            ViewBag.IntervalAsMiliseconds = vm.IntervalAsMiliseconds;

            if (vm.Order == PhotosViewModel.Orders.Random)
            {
                DirMgr.Shuffle(fileArr);
            }

            return View("Photos", fileArr);
        }
    }
}
