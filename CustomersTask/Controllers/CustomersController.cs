using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomersTask.Models;
using CustomersTask.Infra;
using System.Xml.Linq;

namespace CustomersTask.Controllers
{
    public class CustomersController : Controller
    {

        //private CustomersDBEntities db = new CustomersDBEntities();

        // GET: Customers
        public ActionResult Index()
        {

            XMLTransactions xMLTransactions = new XMLTransactions();
            
            return View(xMLTransactions.GetCustomers().ToList());
        }

        [HttpGet]
        public ActionResult Search()
        {

            XMLTransactions xMLTransactions = new XMLTransactions();
            return View("Index", xMLTransactions.GetCustomers().ToList());
        }

        [HttpPost]
        public ActionResult Search(string SearchString)
        {

            XMLTransactions xMLTransactions = new XMLTransactions();
            if (string.IsNullOrEmpty(SearchString))
            {
                return View("Index", xMLTransactions.GetCustomers().ToList());
            }
            else
            {
                return View("Index", xMLTransactions.GetCustomers().Where(p => p.FirstName == SearchString || p.LastName == SearchString).ToList());
            }
        }


        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            XMLTransactions xMLTransactions = new XMLTransactions();
            
            CustomerMetaData customer = xMLTransactions.GetCustomer(id.ToString());
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public ActionResult GetAddressesPartial(int id)
        {
            XMLTransactions xMLTransactions = new XMLTransactions();
            ViewBag.CustomerID = id;
            return PartialView("_Addresses", xMLTransactions.GetAddresses(id));
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerMetaData customer)
        {
            if (ModelState.IsValid)
            {
                XMLTransactions xMLTransactions = new XMLTransactions();

                xMLTransactions.AddCustomer(customer);
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XMLTransactions xMLTransactions = new XMLTransactions();
            CustomerMetaData customer = xMLTransactions.GetCustomer(id.ToString());
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerMetaData customer)
        {
            if (ModelState.IsValid)
            {
                XMLTransactions xMLTransactions = new XMLTransactions();
                xMLTransactions.EditXML(customer);

                //db.Entry(customer).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XMLTransactions xMLTransactions = new XMLTransactions();
            CustomerMetaData customer = xMLTransactions.GetCustomer(id.ToString());

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            XMLTransactions xMLTransactions = new XMLTransactions();
            xMLTransactions.RemoveCustomer(id.ToString());

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
