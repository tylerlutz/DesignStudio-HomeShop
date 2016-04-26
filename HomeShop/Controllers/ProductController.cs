using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeShop.Models;

namespace HomeShop.Controllers
{
    public class ProductController : Controller
    {
        HomeStoreEntities db = new HomeStoreEntities();

        public ActionResult Bathroom()
        {
            Category category = db.Categories.FirstOrDefault(c => c.CategoryName == "Bathroom");
            var products = db.Products.Where(p => p.CategoryID == category.CategoryID);

            return View(products.ToList());
        }

        public ActionResult Kitchen()
        {
            Category category = db.Categories.FirstOrDefault(c => c.CategoryName == "Kitchen");
            var products = db.Products.Where(p => p.CategoryID == category.CategoryID);

            return View(products.ToList());
        }

        public ActionResult Bedroom()
        {
            Category category = db.Categories.FirstOrDefault(c => c.CategoryName == "Bedroom");
            var products = db.Products.Where(p => p.CategoryID == category.CategoryID);

            return View(products.ToList());
        }

        public ActionResult Detail(int id)
        {
            Product product = db.Products.Find(id);

            return View(product);
        }
    }
}