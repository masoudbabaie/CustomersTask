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

namespace CustomersTask.Controllers
{
    public class AddressesController : Controller
    {
        //private CustomersDBEntities db = new CustomersDBEntities();

        // GET: Addresses
        //public ActionResult Index()
        //{
        //    var addresses = db.Addresses.Include(a => a.City).Include(a => a.Customer);
        //    return View(addresses.ToList());
        //}

        // GET: Addresses/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Address address = db.Addresses.Find(id);
        //    if (address == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(address);
        //}

        // GET: Addresses/Create
        public ActionResult Create(int id)
        {
            XMLTransactions xMLTransactions = new XMLTransactions();

            ViewBag.CityID = new SelectList(xMLTransactions.GetCities(), "ID", "CityName");
            ViewBag.CustomerID = id;
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddressMetaData address)
        {
            XMLTransactions xMLTransactions = new XMLTransactions();

            if (ModelState.IsValid)
            {
                xMLTransactions.AddAddress(address);
                return RedirectToAction(actionName: "Details", controllerName: "Customers", routeValues: new { id = address.CustomerID });
            }


            ViewBag.CityID = new SelectList(xMLTransactions.GetCities(), "ID", "CityName");
            ViewBag.CustomerID = address.CustomerID;
            return View(address);
        }

        // GET: Addresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XMLTransactions xMLTransactions = new XMLTransactions();
            AddressMetaData address = xMLTransactions.GetAddress(id.ToString());

            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(xMLTransactions.GetCities(), "ID", "CityName", address.CityID);
            ViewBag.CustomerID = address.CustomerID;
            ViewBag.ID = address.ID;
            return View(address);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddressMetaData address)
        {
            XMLTransactions xMLTransactions = new XMLTransactions();

            if (ModelState.IsValid)
            {
                xMLTransactions.EditAddress(address);

                return RedirectToAction(actionName:"Details", controllerName:"Customers", routeValues: new { id = address.CustomerID });
            }
            ViewBag.CityID = new SelectList(xMLTransactions.GetCities(), "ID", "CityName", address.CityID);
            ViewBag.CustomerID = address.CustomerID; 
            return View(address);
        }

        // GET: Addresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            XMLTransactions xMLTransactions = new XMLTransactions();
            AddressMetaData address = xMLTransactions.GetAddress(id.ToString());

            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            XMLTransactions xMLTransactions = new XMLTransactions();
            int ID = xMLTransactions.GetAddress(id.ToString()).CustomerID;

            xMLTransactions.RemoveAddress(id.ToString());
            return RedirectToAction(actionName: "Details", controllerName: "Customers", routeValues: new { id = ID });
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
