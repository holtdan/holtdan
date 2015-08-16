using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HoltDan.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            DropboxRestAPI.Client client = new DropboxRestAPI.Client(new DropboxRestAPI.Options
            {
                AccessToken = "vBgoZGNyPIMAAAAAAAA0GTo_t9tHdiE_f4EMOGNoelpkTKu7-bWzVNxAN9rdeM4E"
            });
            var what = await client.Core.Metadata.MetadataAsync("/Art", include_media_info: true);

            foreach (var c in what.contents)
            {
                var x = await client.Core.Metadata.SharesAsync(c.path);
            }
            //var token = client.Core.OAuth2.TokenAsync(authCode);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}