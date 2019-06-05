﻿using Cinemonito.Entitys;
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
                var chairGen = this.getChairGen(idMovieByRoom);
                var chairPre = this.getChairPre(idMovieByRoom);
                ViewBag.chairGen = chairGen;
                ViewBag.chairPre = chairPre;
                var model = new List<ChairEntity>();
                // var silla = new ChairEntity();
                //model.Add(silla);
                List<int> arrayChairSelected = new List<int>();
                ViewBag.chairSelected = model;
                ViewBag.arrayChairSelected = arrayChairSelected;
                return View();
            }
        }

        public ActionResult selectChair(int idChair, int idMovieByRoom, int chairSelectedId, string chairSelectedArray, bool delete)
        {
            using (var db = new CinemonitoEntities())
            {
                List<int> arrayChairSelected = System.Web.Helpers.Json.Decode<List<int>>(chairSelectedArray);
                if (delete)
                {
                    arrayChairSelected = arrayChairSelected.Where(chair => chair != chairSelectedId).ToList();
                } else
                {
                    if (chairSelectedId != -1)
                    {
                        arrayChairSelected.Add(chairSelectedId);
                    }
                }
                List<ChairEntity> chairGen = this.getChairGen(idMovieByRoom);
                List<ChairEntity> chairPre = this.getChairPre(idMovieByRoom);
                var model = new List<ChairEntity>();
                List<ChairEntity> allChair = chairGen.Concat(chairPre).ToList();
                chairGen.Clear();
                chairPre.Clear();
                var valorSillas = 0;
                foreach (ChairEntity item in allChair)
                {
                    foreach (int ids in arrayChairSelected)
                    {
                        if (item.Id == ids)
                        {

                            model.Add(item);
                            item.isNotAvalibleLocal = true;
                            valorSillas = valorSillas + item.price;
                        }
                    }
                    if (item.idTypeChair == 1)
                    {
                        chairGen.Add(item);
                    }
                    else
                    {
                        chairPre.Add(item);
                    }
                }
                ViewBag.totalPrice = valorSillas;
                ViewBag.chairGen = chairGen;
                ViewBag.chairPre = chairPre;
                ViewBag.chairSelected = model;
                ViewBag.arrayChairSelected = arrayChairSelected;
                return View("searchChair");
            }
        }

        public ActionResult buyWindowTickets(int idMovieByRoom, string chairSelectedArray )
        {
            using (var db = new CinemonitoEntities())
            {
                var isValidsChairs = new List<ChairEntity>();
                var isNotValidsChairs = new List<ChairEntity>();

                List<int> arrayChairSelected = System.Web.Helpers.Json.Decode<List<int>>(chairSelectedArray);
                bool isValid = true;
                foreach (int ids in arrayChairSelected)
                {
                    var chairSelect = db.ChairByMovie.FirstOrDefault(x => x.Id == ids);
                    ChairEntity chairAllData = this.getOneChair(idMovieByRoom, ids);

                    if ((bool)chairSelect.IsAvailable) {
                        isValidsChairs.Add(chairAllData);

                        chairSelect.IsAvailable = false;
                    } else {
                        isNotValidsChairs.Add(chairAllData);


                        isValid = false;
                    }
                }
                if (isValid)
                {
                    db.SaveChanges();
                }

                return View("buyWindowTickets");
            }
        }

        private ChairEntity getOneChair(int idMovieByRoom, int idChair)
        {
            using (var db = new CinemonitoEntities())
            {
                return (from MoviesByRoom in db.MoviesByRoom
                        join ChairByMovie in db.ChairByMovie on MoviesByRoom.IdMovieByRoom equals ChairByMovie.IdMovieByRoom
                        join Chair in db.Chair on ChairByMovie.IdChair equals Chair.Id
                        join TypeChair in db.TypeChair on Chair.IdTypeChair equals TypeChair.Id
                        where ChairByMovie.IdMovieByRoom.Equals(idMovieByRoom)
                        && ChairByMovie.IdChair == idChair
                        select new ChairEntity
                        {
                            Id = ChairByMovie.Id,
                            idChair = (int)Chair.Id,
                            isAvalible = (bool)ChairByMovie.IsAvailable,
                            idMovieByRoom = (int)ChairByMovie.IdMovieByRoom,
                            idTypeChair = (int)Chair.IdTypeChair,
                            location = Chair.Location,
                            price = (int)TypeChair.Price,
                            description = TypeChair.Description
                        }).FirstOrDefault();
            }
        }


        private List<ChairEntity> getChairPre(int idMovieByRoom)
        {
            using (var db = new CinemonitoEntities())
            {
                return (from MoviesByRoom in db.MoviesByRoom
                        join ChairByMovie in db.ChairByMovie on MoviesByRoom.IdMovieByRoom equals ChairByMovie.IdMovieByRoom
                        join Chair in db.Chair on ChairByMovie.IdChair equals Chair.Id
                        join TypeChair in db.TypeChair on Chair.IdTypeChair equals TypeChair.Id
                        where ChairByMovie.IdMovieByRoom.Equals(idMovieByRoom)
                        && Chair.IdTypeChair == ((int)2)
                        select new ChairEntity
                        {
                            Id = ChairByMovie.Id,
                            idChair = (int)Chair.Id,
                            isAvalible = (bool)ChairByMovie.IsAvailable,
                            idMovieByRoom = (int)ChairByMovie.IdMovieByRoom,
                            idTypeChair = (int)Chair.IdTypeChair,
                            location = Chair.Location,
                            price = (int)TypeChair.Price,
                            description = TypeChair.Description
                        }).ToList();
            }
        }

        private List<ChairEntity> getChairGen(int idMovieByRoom)
        {
            using (var db = new CinemonitoEntities())
            {
                return (from MoviesByRoom in db.MoviesByRoom
                 join ChairByMovie in db.ChairByMovie on MoviesByRoom.IdMovieByRoom equals ChairByMovie.IdMovieByRoom
                 join Chair in db.Chair on ChairByMovie.IdChair equals Chair.Id
                join TypeChair in db.TypeChair on Chair.IdTypeChair equals TypeChair.Id
                where ChairByMovie.IdMovieByRoom == idMovieByRoom
                 && Chair.IdTypeChair == ((int)1)
                 select new ChairEntity
                 {
                     Id = ChairByMovie.Id,
                     idChair = (int)Chair.Id,
                     isAvalible = (bool)ChairByMovie.IsAvailable,
                     idMovieByRoom = (int)ChairByMovie.IdMovieByRoom,
                     idTypeChair = (int)Chair.IdTypeChair,
                     location = Chair.Location,
                     price = (int)TypeChair.Price,
                     description = TypeChair.Description
                 }).ToList();
            }
        }
    }
}