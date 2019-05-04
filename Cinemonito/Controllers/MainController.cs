using Cinemonito.Entitys;
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
                var data = this.getEmployess();
                return View(data);
            }
        }

        public ActionResult movies()
        {
            using (var db = new CinemonitoEntities())
            {
                var data = db.Movie.ToList();
                return View(data);
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
                var data = this.getEmployess();
                var mutliplexList = db.Headquarters.ToList();
                var positionList = db.Position.ToList();
                ViewBag.mutliplexList = mutliplexList;
                ViewBag.positionList = positionList;
                //var item2 = db.Employee.ToList();
                return View("employees", data);
            }
        }

        public ActionResult deleteMovie(int id)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Movie.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    db.Movie.Remove(item);
                    db.SaveChanges();
                }
                var data = db.Movie.ToList();
                return View("movies", data);
            }
        }

        public ActionResult editEmployee(int id)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Employee.Where(x => x.Id == id).First();
                var mutliplexList = db.Headquarters.ToList();
                var positionList = db.Position.ToList();
                ViewBag.mutliplexList = mutliplexList;
                ViewBag.positionList = positionList;
                return View(item);
            } 
        }

        public ActionResult editMovie(int id)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Movie.Where(x => x.Id == id).First();
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult editMovie(Movie model)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Movie.Where(x => x.Id == model.Id).First();
                item.Classification = model.Classification;
                item.Name = model.Name;
                item.Synopsis = model.Synopsis;
                db.SaveChanges();
                var data = db.Movie.ToList();
                return View("movies", data);
            }
        }

        [HttpPost]
        public ActionResult editEmployee(Employee model)
        {
            using (var db = new CinemonitoEntities())
            {
                var item = db.Employee.Where(x => x.Id == model.Id).First();
                item.Name = model.Name;
                item.Identification = model.Identification;
                item.Password = model.Password;
                item.Phone = model.Phone;
                item.ContractDateInit = model.ContractDateInit;
                item.ContractDateEnd = model.ContractDateEnd;
                item.Salary = model.Salary;
                item.IdHeadquarter = model.IdHeadquarter;
                item.IdPosition = model.IdPosition;
                db.SaveChanges();
                var employee = this.getEmployess();
                return View("employees", employee);
            }
        }

        public ActionResult createMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult createMovie(Movie model)
        {
            using (var db = new CinemonitoEntities())
            {
                var movies = db.Set<Movie>();
                movies.Add(new Movie
                {
                    Name = model.Name,
                    Classification = model.Classification,
                    Synopsis = model.Synopsis
                });
                db.SaveChanges();
                var data = db.Movie.ToList();
                return View("movies", data);
            }
        }

        public ActionResult createEmployee()
        {
            using (var db = new CinemonitoEntities())
            {
                var mutliplexList = db.Headquarters.ToList();
                var positionList = db.Position.ToList();
                ViewBag.mutliplexList = mutliplexList;
                ViewBag.positionList = positionList;
                return View();
            }
        }
        [HttpPost]
        public ActionResult createEmployee(Employee model)
        {
            using (var db = new CinemonitoEntities())
            {

                var customers = db.Set<Employee>();
                customers.Add(new Employee {
                    Identification = model.Identification,
                    Name = model.Name,
                    Phone = model.Phone,
                    ContractDateInit = model.ContractDateInit,
                    ContractDateEnd = model.ContractDateEnd,
                    Salary = model.Salary,
                    IdHeadquarter = model.IdHeadquarter,
                    IdPosition = model.IdPosition,
                    Password = model.Password,
                });
                db.SaveChanges();
                var employeeResponse = this.getEmployess();
                var mutliplexList = db.Headquarters.ToList();
                var positionList = db.Position.ToList();
                return View("employees", employeeResponse);
            }
        }

        public ActionResult selectTicketsOption(int id)
        {
            using (var db = new CinemonitoEntities())
            {
                var datos = (from MoviesByRoom in db.MoviesByRoom
                             join Room in db.Room on MoviesByRoom.IdRoom equals Room.Id
                             join Headquarters in db.Headquarters on Room.IdHeadquarter equals Headquarters.Id
                             where MoviesByRoom.IdMovie.Equals(id)
                             select new
                             {
                                 name = Headquarters.Name,
                                 id = Headquarters.Id,
                                 address = Headquarters.Address,
                                 idMovie = MoviesByRoom.IdMovie,
                                 idRoom = MoviesByRoom.IdRoom
                             }).ToList();
                ViewBag.listMultiplex = datos;
                return View();
            }
        }

        public ActionResult selectRoom(MultiplexEntity multiplex)
        {
            using (var db = new CinemonitoEntities())
            {
                var datos = (from MoviesByRoom in db.MoviesByRoom
                             join Movie in db.Movie on MoviesByRoom.IdMovie equals Movie.Id
                             where MoviesByRoom.IdMovie.Equals(multiplex.idMovie) && MoviesByRoom.IdMovie.Equals(multiplex.idRoom)
                             select new Entitys.MoviesByRoom
                             {
                                 idMovie = (int)Movie.Id,
                                 idRoom = (int)MoviesByRoom.IdRoom,
                                 nameMovie = Movie.Name,
                                 Horary = MoviesByRoom.Horary
                             }).ToList();
                multiplex.nameMovie = datos[0].nameMovie;
                ViewBag.listMultiplex = datos;
                return View();

            }
        }

        public object getEmployess()
        {
            using (var db = new CinemonitoEntities())
            {
                var datos = (from Employee in db.Employee
                             join Position in db.Position on Employee.IdPosition equals Position.Id
                             join Headquarters in db.Headquarters on Employee.IdHeadquarter equals Headquarters.Id
                             select new EmployeesEntity
                             {
                                 id = Employee.Id.ToString(),
                                 identification = Employee.Identification,
                                 name = Employee.Name,
                                 phone = Employee.Phone,
                                 contractDateInit = Employee.ContractDateInit.ToString(),
                                 contractDateEnd = Employee.ContractDateEnd.ToString(),
                                 salary = Employee.Salary.ToString(),
                                 headquarterName = Headquarters.Name,
                                 password = Employee.Password,
                                 role = Position.Position1,
                             }).ToList();
                return datos;
            }
        }

        

        
    }
}