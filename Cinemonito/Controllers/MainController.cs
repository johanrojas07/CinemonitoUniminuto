using Cinemonito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinemonito.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult buyTickets()
        {
            using (var db = new CinemonitoEntities())
            {
                return View(from Movie in db.Movie.ToList() select Movie);
            }
        }

        public ActionResult buySnacks()
        {
            return View();
        }

        public ActionResult employees()
        {
            using (var db = new CinemonitoEntities())
            {
                return View(from Employee in db.Employee.ToList() select Employee);
            }
        }
    }
}