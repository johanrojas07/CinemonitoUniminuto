using Cinemonito.Entitys;
using Cinemonito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinemonito.Controllers
{
    public class TicketsController : Controller
    {


        public ActionResult buyTickets()
        {
            using (var db = new CinemonitoEntities())
            {
                return View(from Movie in db.Movie.ToList() select Movie);
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
                             select new MultiplexEntity
                             {
                                 name = Headquarters.Name,
                                 id = (int)Headquarters.Id,
                                 address = Headquarters.Address,
                                 idMovie = (int)MoviesByRoom.IdMovie,
                                 idRoom = (int)MoviesByRoom.IdRoom
                             }).ToList();


                ViewBag.listMultiplex = datos;
                return View();
            }
        }

        public ActionResult selectRoom(int idMovie, int idRoom)
        {
            using (var db = new CinemonitoEntities())
            {
                var datos = (from MoviesByRoom in db.MoviesByRoom
                             join Movie in db.Movie on MoviesByRoom.IdMovie equals Movie.Id
                             join Room in db.Room on MoviesByRoom.IdRoom equals Room.Id
                             where MoviesByRoom.IdMovie.Equals(idMovie) && MoviesByRoom.IdMovie.Equals(idRoom)
                             select new Entitys.MoviesByRoom
                             {
                                 id = (int)MoviesByRoom.IdMovieByRoom,
                                 idMovie = idMovie,
                                 idRoom = idRoom,
                                 nameRoom = Room.Name,
                                 nameMovie = Movie.Name,
                                 horary = MoviesByRoom.Horary
                             }).ToList();
                //multiplex.nameMovie = datos[0].nameMovie;
                ViewBag.MoviesByRoom = datos;
                return View();

            }
        }

        public object searchChair(int idMovie, int idRoom, int idMovieByRoom)
        {
            using (var db = new CinemonitoEntities())
            {
                var chairGen = (from MoviesByRoom in db.MoviesByRoom
                                join ChairByMovie in db.ChairByMovie on MoviesByRoom.IdMovieByRoom equals ChairByMovie.IdMovieByRoom
                                join Chair in db.Chair on ChairByMovie.IdChair equals Chair.Id
                                where ChairByMovie.IdMovieByRoom == idMovieByRoom
                                && Chair.IdTypeChair == ((int)1)
                                select new ChairEntity
                                {
                                    idChair = (int)Chair.Id,
                                    isAvalible = ChairByMovie.IsAvailable,
                                    idMovieByRoom = (int)ChairByMovie.IdMovieByRoom,
                                    idTypeChair = (int)Chair.IdTypeChair,
                                    location = Chair.Location,
                                }
                    ).ToList();
                var chairPre = (from MoviesByRoom in db.MoviesByRoom
                                join ChairByMovie in db.ChairByMovie on MoviesByRoom.IdMovieByRoom equals ChairByMovie.IdMovieByRoom
                                join Chair in db.Chair on ChairByMovie.IdChair equals Chair.Id
                                where ChairByMovie.IdMovieByRoom.Equals(idMovieByRoom)
                                && Chair.IdTypeChair == ((int)2)
                                select new ChairEntity
                                {
                                    idChair = (int)Chair.Id,
                                    isAvalible = ChairByMovie.IsAvailable,
                                    idMovieByRoom = (int)ChairByMovie.IdMovieByRoom,
                                    idTypeChair = (int)Chair.IdTypeChair,
                                    location = Chair.Location,
                                }
                   ).ToList();
                ViewBag.chairGen = chairGen;
                ViewBag.chairPre = chairPre;
                return View();
            }
        }
    }
}