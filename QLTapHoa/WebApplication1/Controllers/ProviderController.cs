using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProviderController : Controller
    {
        private CNPMEntities db = new CNPMEntities();
        public ActionResult DetailProvider(string id)
        {
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        public ActionResult Index()
        {
            List<Provider> providers = db.Providers.ToList();
            return View(providers);
        }

        public ActionResult CreateProvider()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateProvider(Provider provider)
        {
            if (ModelState.IsValid)
            {

                // Kiểm tra số điện thoại có đúng 10 chữ số không
                if (!IsValidPhoneNumber(provider.phoneProvider))
                {
                    ModelState.AddModelError("phoneProvider", "Số điện thoại phải có đúng 10 chữ số.");
                    return View(provider);
                }

                // Kiểm tra định dạng email hợp lệ không
                if (!IsValidEmail(provider.emailProvider))
                {
                    ModelState.AddModelError("emailProvider", "Email không hợp lệ.");
                    return View(provider);
                }


                // Tăng giá trị idProvider và cộng thêm chuỗi "RV0"
                provider.idProvider = GenerateUniqueNextIdProvider();

                db.Providers.Add(provider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(provider);
        }
        // Phương thức kiểm tra số điện thoại có đúng 10 chữ số
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\d{10}$");
        }

        // Phương thức kiểm tra email có định dạng hợp lệ
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$");
        }

        // Hàm tạo giá trị mới cho idProvider đảm bảo là duy nhất trong cơ sở dữ liệu
        private string GenerateUniqueNextIdProvider()
        {
            string newIdProvider = GenerateNextIdProvider();
            while (IdProviderExistsInDatabase(newIdProvider))
            {
                newIdProvider = GenerateNextIdProvider();
            }

            return newIdProvider;
        }

        // Hàm kiểm tra xem giá trị idProvider đã tồn tại trong cơ sở dữ liệu chưa
        private bool IdProviderExistsInDatabase(string idProvider)
        {
            // Code kiểm tra cơ sở dữ liệu của bạn ở đây
            // Trả về true nếu giá trị tồn tại và false nếu không tồn tại
            return db.Providers.Any(p => p.idProvider == idProvider);
        }

        // Hàm tăng giá trị idProvider và cộng thêm chuỗi "RV0"
        private string GenerateNextIdProvider()
        {
            // Lấy giá trị idProvider mới nhất từ cơ sở dữ liệu hoặc danh sách nhà cung cấp
            string currentIdProvider = GetLatestIdProviderFromDatabase();

            // Nếu không có giá trị idProvider trong cơ sở dữ liệu hoặc danh sách nhà cung cấp, sử dụng giá trị mặc định
            if (string.IsNullOrEmpty(currentIdProvider))
            {
                currentIdProvider = "RV00"; // Giá trị mặc định khi không tìm thấy giá trị trong cơ sở dữ liệu
            }

            // Tách phần số từ chuỗi hiện tại
            int currentNumber = int.Parse(currentIdProvider.Substring(3));

            // Tăng số lên 1
            currentNumber++;

            // Tạo giá trị mới cho idProvider
            string newIdProvider = "PRV" + currentNumber.ToString("D2");

            return newIdProvider;
        }

        // Hàm lấy giá trị idProvider mới nhất từ cơ sở dữ liệu hoặc danh sách nhà cung cấp
        private string GetLatestIdProviderFromDatabase()
        {
            // Code để truy vấn cơ sở dữ liệu hoặc danh sách nhà cung cấp và lấy giá trị idProvider mới nhất
            // Thay thế dòng sau bằng code truy vấn của bạn
            var latestProvider = db.Providers.OrderByDescending(p => p.idProvider).FirstOrDefault();
            return latestProvider?.idProvider;
        }

        public ActionResult EditProvider(string id)
        {
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        [HttpPost]
        public ActionResult EditProvider(Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(provider);
        }


        public ActionResult DeleteProvider(string id)
        {
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        [HttpPost]
        public ActionResult DeleteProviderConfirmed(string id)
        {
            try
            {
                Provider provider = db.Providers.Find(id);
                if (provider == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhà cung cấp để xóa." });
                }
                db.Providers.Remove(provider);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa nhà cung cấp: " + ex.Message });
            }
        }


        // xóa dữ liệu bên product
        [HttpPost]
        public ActionResult DeleteProductConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }


            // Xóa sản phẩm
            db.Products.Remove(product);
            db.SaveChanges();
            return Json(new { success = true });
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