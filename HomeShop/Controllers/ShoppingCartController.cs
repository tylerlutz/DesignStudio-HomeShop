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

        public ActionResult Checkout(int orderID)
        {
            var items = db.ShoppingCartItems.Where(i => i.OrderID == orderID);

            double totalCost;

            foreach(var item in items)
            {

            }

            return View();
        }

        public ActionResult AddItem(int productID, int quantity)
        {
            Product product = new Product();
            ShoppingCartItem cartItem = new ShoppingCartItem();
            bool success = false;
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

        public ActionResult DeleteFromCart(int productID, int orderID)
        {

            var items = db.ShoppingCartItems.Where(i => i.OrderID == orderID);

            foreach(var item in items)
            {
                if(item.ProductID == productID)
                {
                    db.ShoppingCartItems.Remove(item);
                }
            }
            
            return View("~/Views/ShoppingCart/Cart");
        }

        private CustomerOrder GenerateNewOrder()
        {
            CustomerOrder newOrder = new CustomerOrder();
            newOrder.CustomerID = User.Identity.GetUserId();

            db.CustomerOrders.Add(newOrder);
            db.SaveChanges();

            return newOrder;
        }
    }
}