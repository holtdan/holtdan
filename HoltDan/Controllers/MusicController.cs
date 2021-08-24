using HoltDan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Controllers
{
    public class MusicController : Controller
    {
        string Root()
        {
            return Server.MapPath("~/media/music");
        }
        // GET: Music
        public ActionResult Index()
        {
            return View(new MusicViewModel(Root()));
        }
        public PartialViewResult AlbumList(string artist)
        {
            var path = $"{Root()}\\{artist}";
            return PartialView("_AlbumList",
                (from d in Directory.GetDirectories(path)
                 orderby d
                 select d.Substring(path.Length + 1)
                 ).ToList()
                );
        }
        public PartialViewResult Album(string artist, string album)
        {
            return PartialView("_Album", new DirMgr(Server.MapPath($"/media/music/{artist}/{album}"), Server.MapPath($"/media/music/{artist}/{album}")));
        }
    }
}