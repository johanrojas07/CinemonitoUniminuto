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

        public ActionResult selectRoom(Multiplex multiplex)
        {
            using (var db = new CinemonitoEntities())
            {
                var datos = (from MoviesByRoom in db.MoviesByRoom
                             join Movie in db.Movie on MoviesByRoom.IdMovie equals Movie.Id
                             where MoviesByRoom.IdMovie.Equals(multiplex.idMovie) && MoviesByRoom.IdMovie.Equals(multiplex.idRoom)
                             select new
                             {
                                 nameMovie = Movie.Name,
                                 horary = MoviesByRoom.Horary
                             }).ToList();
                multiplex.nameMovie = datos[0].nameMovie;
                ViewBag.listMultiplex = datos;
                return View();

            }
        }

        public class Multiplex
        {
            public string name, address, nameMovie;
            public int id, idMovie, idRoom;

            public Multiplex()
            {
                this.nameMovie = null;
                this.name = null;
                this.address = null;
                this.id = 0;
                this.idMovie = 0;
                this.idRoom = 0;
            }
        }
        //selectTicketsOption
    }
}