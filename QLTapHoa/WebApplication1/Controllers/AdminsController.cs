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
    public class AdminsController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        public ActionResult InfoAdmin()
        {
            return View();
        }

        // Tạo view cho khách hàng Login
        public ActionResult LoginAdmin()
        {
            return View();
        }
        // Xử lý tìm kiếm UserName, password trong Customer và thông báo
        [HttpPost]
        public ActionResult LoginAcount(Admin _admin)
        {
            // check là admin cần tìm
            var check = db.Admins.Where(s => s.username == _admin.username && s.phone == _admin.phone).FirstOrDefault();
            if (check == null)  //không có KH
            {
                ViewBag.ErrorInfo = "Không có KH này";
                return View("LoginAdmin");
            }
            else
            {   // Có tồn tại KH -> chuẩn bị dữ liệu
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["idAdmin"] = check.idAdmin;
                Session["userName"] = check.username;
                Session["phone"] = check.phone;
                Session["gender"] = check.gender;
                Session["email"] = check.email;

                return RedirectToAction("ProductList", "Products");
            }
        }

        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("LoginAdmin");
        }

        // GET: Admins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idAdmin,username,phone,gender,email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idAdmin,username,phone,gender,email")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
