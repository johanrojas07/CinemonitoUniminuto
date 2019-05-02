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

        public ActionResult delete(int id)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Employee.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    db.Employee.Remove(item);
                    db.SaveChanges();
                }
                var item2 = db.Employee.ToList();
                return View("employees", item2);
            }
        }

        public ActionResult editEmployee(int id)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Employee.Where(x => x.Id == id).First();
                return View(item);
            } 
        }
        [HttpPost]
        public ActionResult editEmployee(Employee model)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Employee.Where(x => x.Id == model.Id).First();
                item.Identification = model.Name;
                item.Password = model.Password;
                db.SaveChanges();
                var item2 = db.Employee.ToList();
                return View("employees", item2);
            }
                
        }

        public ActionResult createEmployee()
        {
            using (var db = new CinemonitoEntities())
            {
                return View();
            }
        }
    }
}