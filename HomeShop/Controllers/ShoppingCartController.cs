using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeShop.Models;
using Microsoft.AspNet.Identity;

namespace HomeShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private int? OrderID;
        private HomeStoreEntities db = new HomeStoreEntities();

        public ActionResult AddItem(int productID, int quantity)
        {
            Product product = new Product();
            ShoppingCartItem cartItem = new ShoppingCartItem();
            Boolean success = false;
            product = db.Products.Find(productID);
            if (OrderID == null)
            {
                OrderID = GenerateNewOrder().OrderID;

                cartItem.OrderID = OrderID;
                cartItem.ProductID = product.ProductID;
                cartItem.Quantity = quantity;
                cartItem.Price = product.Price*quantity;

                db.ShoppingCartItems.Add(cartItem);
                db.SaveChanges();
                success = true;
            }
            else
            {
                cartItem.OrderID = OrderID;
                cartItem.ProductID = product.ProductID;
                cartItem.Quantity = quantity;
                cartItem.Price = product.Price * quantity;

                db.ShoppingCartItems.Add(cartItem);
                db.SaveChanges();
                success = true;
            }

            if (success)
            {
                return View();
            }
            else
            {
                return View("~/Views/Shared/Error");
            }
        }

        public CustomerOrder GenerateNewOrder()
        {
            CustomerOrder newOrder = new CustomerOrder();
            newOrder.CustomerID = User.Identity.GetUserId();

            db.CustomerOrders.Add(newOrder);
            db.SaveChanges();

            return newOrder;
        }
    }
}