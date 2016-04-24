using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "this is homestore.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "find us, meet us, greet us";

            return View();
        }

        public ActionResult ShoppingCart()
        {
            ViewBag.Message = "our shopping cart";

            return View();
        }
        public ActionResult Shop()
        {
            ViewBag.Message = "Shop Here";

            return View();
        }



    }
}