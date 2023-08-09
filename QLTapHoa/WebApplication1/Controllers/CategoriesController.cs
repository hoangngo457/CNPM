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
    public class CategoriesController : Controller
    {
        private CNPMEntities db = new CNPMEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCategory,nameCategory")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.idCategory = GenerateUniqueNextIdCate();

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        private string GenerateUniqueNextIdCate()
        {
            string newIdCate = GenerateNextIdCate();
            while (IdCateExistsInDatabase(newIdCate))
            {
                newIdCate = GenerateNextIdCate();
            }

            return newIdCate;
        }

        private bool IdCateExistsInDatabase(string idCate)
        {
            // Code kiểm tra cơ sở dữ liệu của bạn ở đây
            // Trả về true nếu giá trị tồn tại và false nếu không tồn tại
            return db.Categories.Any(p => p.idCategory == idCate);
        }

        private string GenerateNextIdCate()
        {
            string currentIdCate = GetLatestIdCateFromDatabase();

            if (string.IsNullOrEmpty(currentIdCate))
            {
                currentIdCate = "SP00"; // Giá trị mặc định khi không tìm thấy giá trị trong cơ sở dữ liệu
            }

            // Tách phần số từ chuỗi hiện tại
            int currentNumber = int.Parse(currentIdCate.Substring(3));

            // Tăng số lên 1
            currentNumber++;

            // Tạo giá trị mới cho idProduct
            string newIdCate = "CT" + currentNumber.ToString("D2");

            return newIdCate;
        }


        private string GetLatestIdCateFromDatabase()
        {
            var latestCate = db.Categories.OrderByDescending(p => p.idCategory).FirstOrDefault();
            return latestCate?.idCategory;
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCategory,nameCategory")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
