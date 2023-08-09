using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ImportTicketsController : Controller
    {
        private CNPMEntities db = new CNPMEntities();

        // GET: ImportTickets
        public ActionResult Index()
        {
            var importTickets = db.ImportTickets.Include(i => i.Product).Include(i => i.Provider);
            return View(importTickets.ToList());
        }

        // GET: ImportTickets/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTicket importTicket = db.ImportTickets.Find(id);
            if (importTicket == null)
            {
                return HttpNotFound();
            }
            return View(importTicket);
        }

        // GET: ImportTickets/Create
        public ActionResult Create()
        {
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct");
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider");
            return View();
        }

        // POST: ImportTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idImport,idProduct,date,quantity,addressDelivery,price,totalPrice,idProvider")] ImportTicket importTicket)
        {
            if (ModelState.IsValid)
            {
                db.ImportTickets.Add(importTicket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", importTicket.idProduct);
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider", importTicket.idProvider);
            return View(importTicket);
        }

        // GET: ImportTickets/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTicket importTicket = db.ImportTickets.Find(id);
            if (importTicket == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", importTicket.idProduct);
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider", importTicket.idProvider);
            return View(importTicket);
        }

        // POST: ImportTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idImport,idProduct,date,quantity,addressDelivery,price,totalPrice,idProvider")] ImportTicket importTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(importTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", importTicket.idProduct);
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider", importTicket.idProvider);
            return View(importTicket);
        }

        // GET: ImportTickets/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImportTicket importTicket = db.ImportTickets.Find(id);
            if (importTicket == null)
            {
                return HttpNotFound();
            }
            return View(importTicket);
        }

        // POST: ImportTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ImportTicket importTicket = db.ImportTickets.Find(id);
            db.ImportTickets.Remove(importTicket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
