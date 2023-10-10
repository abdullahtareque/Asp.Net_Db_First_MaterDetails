using EvidenceTest.Models;
using EvidenceTest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidenceTest.Controllers
{
    public class CustomerController : Controller
    {
        private EvidenceDBEntities db = new EvidenceDBEntities();
        List<Customer> cst = new List<Customer>();
        List<Order> ord = new List<Order>();


        public ActionResult Index(int? customerID = null)
        {
            CustOrderVM co = null;
            if (customerID != null)
            {
                ord = (from d in db.Orders where d.CustomerID == customerID select d).ToList();
                Customer c = db.Customers.Find(customerID);
                co = new CustOrderVM { CustomerID = c.CustomerID };
            }

            var items = db.items.ToList();
            var itemViewModels = items.Select(item => new CustOrderVM
            {
                ItemID = item.ItemID,
                ItemName = item.ItemName
            }).ToList();
            ViewBag.ItemList = new SelectList(itemViewModels, "ItemId", "ItemName");


            ViewBag.Records = ord;

            TempData["ord"] = ord;

            return View(co);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult index(CustOrderVM co, string ButtonType)
        {
            if (ButtonType == "Add")
            {
                AddM(co);
                return PartialView("_PartialPage1");
            }
            else if (ButtonType == "Save")
                SaveM(co);
            else
                DeleteM(co.CustomerID);
            return Json(new { url = Url.Action("list") });
        }

        [Route("ExamTest")]
        public ActionResult List()
        {
            TempData["sdt"] = "";
            List<CustOrderVM> c = new List<CustOrderVM>();
            var a = db.Customers.ToList();
            foreach (var m2 in a)
            {
                c.Add(new CustOrderVM { CustomerID = m2.CustomerID, CutomerName = m2.CutomerName, BillingAddress = m2.BillingAddress, Imagepath=m2.Imagepath });
            }
            return View(c);
        }


        public void AddM(CustOrderVM co)
        {
            ord = TempData["ord"] as List<Order>;
            if (ord == null)
                ord = new List<Order>();
            ord.Add(new Order() { OrderID = co.OrderID, OrderNo=co.OrderNo, ItemID=co.ItemID, orderDate = co.orderDate, OrderStatus = co.OrderStatus=true });

            ViewBag.records = ord;
            TempData["ord"] = ord;
        }

        public void SaveM(CustOrderVM co)
        {
            DeleteM(co.CustomerID);
            Customer c = new Customer() { CustomerID = co.CustomerID, CutomerName = co.CutomerName, BillingAddress = co.BillingAddress, Imagepath = co.Imagepath };
            db.Customers.Add(c);
            db.SaveChanges();
            ord = TempData["ord"] as List<Order>;
            foreach (Order d in ord)
            {
                Order o = new Order() { OrderID = d.OrderID, OrderNo = co.OrderNo, ItemID = co.ItemID, CustomerID = co.CustomerID, orderDate = d.orderDate, OrderStatus = d.OrderStatus };
                db.Orders.Add(o);
                db.SaveChanges();
            }

            TempData["ord"] = "";
            Session["ord"] = "";
        }

        public void DeleteM(int CustomerID)
        {
            db.Database.ExecuteSqlCommand($"delete Orders where CustomerID='{CustomerID}'");
            db.Database.ExecuteSqlCommand($"delete Customers where CustomerId='{CustomerID}'");
            db.SaveChanges();
        }

        public ActionResult Delete(int id)
        {
            var customer = db.Customers.Include(c => c.Orders).FirstOrDefault(c => c.CustomerID == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            db.Orders.RemoveRange(customer.Orders);
            db.Customers.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("List");
        }


        // New Added
        public ActionResult Edit(int id)
        {
            var customer = db.Customers.Find(id);
            var orders = db.Orders.Where(o => o.CustomerID == id).ToList();

            var viewModel = new CustOrderVM
            {
                Customer = customer,
                Orders = orders

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(CustOrderVM viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.Customer).State = EntityState.Modified;
                db.SaveChanges();


                foreach (var order in viewModel.Orders)
                {
                    db.Entry(order).State = EntityState.Modified;
                }
                db.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("List");
            }
        }







    }
}
    
