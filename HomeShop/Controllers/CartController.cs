using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Stripe;
using System.Data.Entity;

namespace HomeShop.Controllers
{
    public class CartController : Controller
    {
        private HomeStoreEntities db = new HomeStoreEntities();

        public ActionResult ShoppingCart()
        {
            if (TempData["orderid"] == null)
            {
                CustomerOrder order = GenerateNewOrder();
                TempData["orderid"] = order.OrderID;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ShoppingCartViewModel model = new ShoppingCartViewModel();
                decimal? total = 0;

                int id = (int)(TempData["orderid"]);

                var items = db.ShoppingCartItems.Where(i => i.OrderID == id);
                model.CartItems = items.ToList();
                foreach (var item in items)
                {
                    total += item.Price * item.Quantity;
                }

                model.TotalCost = total;
                model.OrderID = (int)(TempData["orderid"]);

                return View(model);
            }
        }

        public ActionResult Checkout(int id)
        {
            TempData["orderid"] = id;
            var items = db.ShoppingCartItems.Where(i => i.OrderID == id);

            decimal? totalCost = 0;

            foreach (var item in items)
            {
                totalCost += item.Price * item.Quantity;
            }

            CustomerOrder order = db.CustomerOrders.Find(id);
            order.TotalCost = totalCost;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.ShippingID = new SelectList(db.ShippingTypes, "ShippingID", "ShippingName");

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout([Bind(Include = "OrderID, TotalCost, ShippingID, Token")] CustomerOrder model)
        {
            decimal? newTotalCost = 0;
            decimal? shippingCost = 0;

            int? id = (int)(TempData["orderid"]);

            ShippingType shippingType = db.ShippingTypes.Find(model.ShippingID);

            shippingCost = shippingType.ShippingCost;
            newTotalCost = model.TotalCost + shippingCost;
            model.TotalCost = newTotalCost;

            var chargeID = await ProcessPayment(model);

            CustomerOrder order = db.CustomerOrders.Find(id);
            order.TransactionID = chargeID;
            order.ShippingID = model.ShippingID;

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return View("PaymentSuccessful");
        }

        private async Task<string> ProcessPayment(CustomerOrder model)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string userEmail = currentUser.Email;

            return await Task.Run(() =>
            {
                var myCharge = new StripeChargeCreateOptions
                {
                    Amount = (int)(model.TotalCost * 100),
                    Currency = "usd",
                    Description = "Order Number: " + model.OrderID.ToString(),
                    ReceiptEmail = userEmail,
                    Source = new StripeSourceOptions
                    {
                        TokenId = model.Token
                    }
                };
                var chargeService = new StripeChargeService("sk_test_Vj4vnMHdXkmwYAB4nvO0R6ng");
                var stripeCharge = chargeService.Create(myCharge);
                return stripeCharge.Id;
            });
        }

        public ActionResult AddItem(int? productID, int quantity)
        {
            Product product = new Product();
            ShoppingCartItem cartItem = new ShoppingCartItem();
            bool success = false;
            product = db.Products.Find(productID);
            if (TempData["orderid"] == null)
            {
                int OrderID = GenerateNewOrder().OrderID;

                cartItem.OrderID = OrderID;
                cartItem.ProductID = product.ProductID;
                cartItem.Quantity = quantity;
                cartItem.Price = product.Price * quantity;

                db.ShoppingCartItems.Add(cartItem);
                db.SaveChanges();
                success = true;
                TempData["orderid"] = OrderID;
            }
            else
            {
                cartItem.OrderID = (int)(TempData["orderid"]);
                cartItem.ProductID = product.ProductID;
                cartItem.Quantity = quantity;
                cartItem.Price = product.Price * quantity;

                db.ShoppingCartItems.Add(cartItem);
                db.SaveChanges();
                success = true;
            }

            if (success)
            {
                return RedirectToAction("ShoppingCart", "Cart");
            }
            else
            {
                return View("~/Views/Shared/Error");
            }
        }

        public ActionResult DeleteFromCart(int productID, int orderID)
        {

            var items = db.ShoppingCartItems.Where(i => i.OrderID == orderID);

            foreach (var item in items)
            {
                if (item.ProductID == productID)
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