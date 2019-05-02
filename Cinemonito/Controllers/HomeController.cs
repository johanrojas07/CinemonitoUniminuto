using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cinemonito.Models;

namespace Cinemonito.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new CinemonitoEntities())
            {
                return View(from Employee in db.Employee.ToList() select Employee);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}