using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StatisticsController : Controller
    {
        private CNPMEntities db = new CNPMEntities();

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            var chartData = products.Select(p => new { ProductName = p.nameProduct, Quantity = p.quantity }).ToList();
            ViewBag.ChartData = chartData;

            return View();
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