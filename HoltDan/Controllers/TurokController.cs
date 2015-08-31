using HoltDan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Controllers
{
    public class TurokController : Controller
    {
        // GET: Turok
        public ActionResult Index()
        {
            //var files = DirMgr.GetFileNames(Server.MapPath("~/App_Data/Turok/images"),"*.*");
            //ViewBag.Files = files;
            return View();
        }
        public ActionResult Games()
        {
            return View();
        }
        public ActionResult Stuff()
        {
            return View();
        }
        public ActionResult Comics()
        {
            return View();
        }
    }
}