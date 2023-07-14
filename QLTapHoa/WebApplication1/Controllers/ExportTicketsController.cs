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
    public class ExportTicketsController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: ExportTickets
        public ActionResult Index()
        {
            var exportTickets = db.ExportTickets.Include(e => e.Product);
            return View(exportTickets.ToList());
        }

        // GET: ExportTickets/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExportTicket exportTicket = db.ExportTickets.Find(id);
            if (exportTicket == null)
            {
                return HttpNotFound();
            }
            return View(exportTicket);
        }

        // GET: ExportTickets/Create
        public ActionResult Create()
        {
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct");
            return View();
        }

        // POST: ExportTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idExport,idProduct,quantity,date,addressDelivery,price,totalPrice")] ExportTicket exportTicket)
        {
            if (ModelState.IsValid)
            {
                db.ExportTickets.Add(exportTicket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", exportTicket.idProduct);
            return View(exportTicket);
        }

        // GET: ExportTickets/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExportTicket exportTicket = db.ExportTickets.Find(id);
            if (exportTicket == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", exportTicket.idProduct);
            return View(exportTicket);
        }

        // POST: ExportTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idExport,idProduct,quantity,date,addressDelivery,price,totalPrice")] ExportTicket exportTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exportTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProduct = new SelectList(db.Products, "idProduct", "nameProduct", exportTicket.idProduct);
            return View(exportTicket);
        }

        // GET: ExportTickets/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExportTicket exportTicket = db.ExportTickets.Find(id);
            if (exportTicket == null)
            {
                return HttpNotFound();
            }
            return View(exportTicket);
        }

        // POST: ExportTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ExportTicket exportTicket = db.ExportTickets.Find(id);
            db.ExportTickets.Remove(exportTicket);
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
