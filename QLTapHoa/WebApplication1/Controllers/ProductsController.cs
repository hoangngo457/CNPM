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
    public class ProductsController : Controller
    {
        private CNPMEntities db = new CNPMEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Provider);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.Categories, "idCategory", "nameCategory");
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProduct,nameProduct,idCategory,price,quantity,imageProduct,status,idProvider")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.idProduct = GenerateUniqueNextIdProduct();

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategory = new SelectList(db.Categories, "idCategory", "nameCategory", product.idCategory);
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider", product.idProvider);
            return View(product);
        }

        private string GenerateUniqueNextIdProduct()
        {
            string newIdProduct = GenerateNextIdProduct();
            while (IdProductExistsInDatabase(newIdProduct))
            {
                newIdProduct = GenerateNextIdProduct();
            }

            return newIdProduct;
        }

        // Hàm kiểm tra xem giá trị idProduct đã tồn tại trong cơ sở dữ liệu chưa
        private bool IdProductExistsInDatabase(string idProduct)
        {
            // Code kiểm tra cơ sở dữ liệu của bạn ở đây
            // Trả về true nếu giá trị tồn tại và false nếu không tồn tại
            return db.Products.Any(p => p.idProduct == idProduct);
        }

        // Hàm tăng giá trị idProduct và cộng thêm chuỗi "SP0"
        private string GenerateNextIdProduct()
        {
            // Lấy giá trị idProduct mới nhất từ cơ sở dữ liệu hoặc danh sách sản phẩm
            string currentIdProduct = GetLatestIdProductFromDatabase();

            // Nếu không có giá trị idProduct trong cơ sở dữ liệu hoặc danh sách sản phẩm, sử dụng giá trị mặc định
            if (string.IsNullOrEmpty(currentIdProduct))
            {
                currentIdProduct = "SP00"; // Giá trị mặc định khi không tìm thấy giá trị trong cơ sở dữ liệu
            }

            // Tách phần số từ chuỗi hiện tại
            int currentNumber = int.Parse(currentIdProduct.Substring(3));

            // Tăng số lên 1
            currentNumber++;

            // Tạo giá trị mới cho idProduct
            string newIdProduct = "SP" + currentNumber.ToString("D2");

            return newIdProduct;
        }

        // Hàm lấy giá trị idProductr mới nhất từ cơ sở dữ liệu hoặc danh sách sản phẩm
        private string GetLatestIdProductFromDatabase()
        {
            // Code để truy vấn cơ sở dữ liệu hoặc danh sách nhà cung cấp và lấy giá trị idProduct mới nhất
            // Thay thế dòng sau bằng code truy vấn của bạn
            var latestProduct = db.Products.OrderByDescending(p => p.idProduct).FirstOrDefault();
            return latestProduct?.idProduct;
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategory = new SelectList(db.Categories, "idCategory", "nameCategory", product.idCategory);
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider", product.idProvider);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProduct,nameProduct,idCategory,price,quantity,imageProduct,status,idProvider")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.Categories, "idCategory", "nameCategory", product.idCategory);
            ViewBag.idProvider = new SelectList(db.Providers, "idProvider", "nameProvider", product.idProvider);
            return View(product);
        }

        //GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteConfirmed(string id)
        {
            //try
            //{
            //    Product product = db.Products.Find(id);
            //    db.Products.Remove(product);
            //    db.SaveChanges();

            //    return Json(new { success = true });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa sản phẩm: " + ex.Message });
            //}

            var model = db.Products.Find(id);
            db.Products.Remove(model);
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
