using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginAdminsController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();
        // GET: LoginAdmins
        public ActionResult Index()
        {
            return View();
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
            // check là khách hàng cần tìm
            var check = db.Admins.Where(s => s.username == _admin.username && s.phone == _admin.phone).FirstOrDefault();
            if (check == null)  //không có KH
            {
                ViewBag.ErrorInfo = "Không có KH này";
                return View("LoginAdmin");
            }
            else
            {   // Có tồn tại KH -> chuẩn bị dữ liệu đưa về lại ShowCart.cshtml
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["idAdmin"] = check.idAdmin;
                Session["userName"] = check.username;
                Session["phone"] = check.phone;
                Session["gender"] = check.gender;
                Session["email"] = check.email;
                
                return RedirectToAction("InfoAdmin");
            }
        }

        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("LoginAdmin");
        }
    }
}