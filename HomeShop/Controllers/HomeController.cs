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

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult ShoppingCart()
        {

            return View();
        }
        public ActionResult Shop()
        {
            ViewBag.Message = "Shop Here";

            return View();
        }



    }
}