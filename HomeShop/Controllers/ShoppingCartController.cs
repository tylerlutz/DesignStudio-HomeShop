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

namespace HomeShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private HomeStoreEntities db = new HomeStoreEntities();

        public ActionResult ShoppingCart()
        {
            if(TempData["orderid"] == null)
            {
                return View("EmptyCart");
            } else
            {
                var items = db.CustomerOrders.Where(i => i.OrderID == (int)(TempData["orderid"]));
                return View(items.ToList());
            }
        }

        public ActionResult Checkout(int orderID)
        {
            TempData["orderid"] = orderID;
            var items = db.ShoppingCartItems.Where(i => i.OrderID == orderID);

            decimal? totalCost = 0;

            foreach(var item in items)
            {
                totalCost += item.Price * item.Quantity;
            }

            CustomerOrder order = db.CustomerOrders.Find(orderID);
            order.TotalCost = totalCost;
            db.SaveChanges();

            return View(order);
        }

        [HttpPost]
        public async Task<ActionResult>Checkout(CustomerOrder model, int shippingID)
        {
            decimal? newTotalCost = 0;
            decimal? shippingCost = 0;

            ShippingType shippingType = db.ShippingTypes.Find(shippingID);

            shippingCost = shippingType.ShippingCost;
            newTotalCost = model.TotalCost + shippingCost;
            model.TotalCost = newTotalCost;

            var chargeID = await ProcessPayment(model);

            CustomerOrder order = db.CustomerOrders.Find((int)TempData["orderid"]);
            order.TransactionID = chargeID;
            order.TotalCost = model.TotalCost;
            order.ShippingID = model.ShippingID;

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

        public ActionResult AddItem(int productID, int quantity)
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
                cartItem.Price = product.Price*quantity;

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