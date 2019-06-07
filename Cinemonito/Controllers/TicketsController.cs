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

        public ActionResult selectTicketsOption(int id, int idClient)
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
                ViewBag.idClient = idClient;
                ViewBag.listMultiplex = datos;
                return View();
            }
        }

        public ActionResult selectRoom(int idMovie, int idRoom, int idClient)
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
                ViewBag.idClient = idClient;
                ViewBag.MoviesByRoom = datos;
                return View();

            }
        }

        public object searchChair(int idMovie, int idRoom, int idMovieByRoom, int idClient)
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

                ViewBag.idClient = idClient;
                ViewBag.chairSelected = model;
                ViewBag.arrayChairSelected = arrayChairSelected;
                return View();
            }
        }

        public ActionResult selectChair(int idChair, int idMovieByRoom, int chairSelectedId, int idClient, string chairSelectedArray, bool delete)
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
                ViewBag.idClient = idClient;
                ViewBag.chairPre = chairPre;
                ViewBag.chairSelected = model;
                ViewBag.arrayChairSelected = arrayChairSelected;
                ViewBag.idMovieByRoom = idMovieByRoom;

                return View("searchChair");
            }
        }

        public ActionResult buyWindowTickets(int idMovieByRoom, int idClient, string chairSelectedArray)
        {
            using (var db = new CinemonitoEntities())
            {
                var isValidsChairs = new List<ChairEntity>();
                var isNotValidsChairs = new List<ChairEntity>();
                var valorSillas = 0;
                var discountSilla = 0;
                var puntosCliente = 0;
                var freeTicketsWithPoints = 0;
                List<int> arrayChairSelected = System.Web.Helpers.Json.Decode<List<int>>(chairSelectedArray);
                bool isValid = true;
                List<int> allValidchair = new List<int>();
                foreach (int ids in arrayChairSelected)
                {
                    var chairSelect = db.ChairByMovie.FirstOrDefault(x => x.Id == ids);
                    ChairEntity chairAllData = this.getOneChair(idMovieByRoom, ids);

                    if ((bool)chairSelect.IsAvailable) {
                        isValidsChairs.Add(chairAllData);
                        allValidchair.Add(ids);
                        valorSillas = valorSillas + chairAllData.price;
                        discountSilla = (discountSilla < chairAllData.price) ? chairAllData.price : discountSilla;
                        puntosCliente = puntosCliente + 10;
                    } else {
                        isNotValidsChairs.Add(chairAllData);
                        isValid = false;
                    }
                }
                if (isValid)
                {
                    this.addPoints(puntosCliente, idClient);
                    freeTicketsWithPoints = this.getFreeTickets(idClient);
                    if (freeTicketsWithPoints >0)
                    {
                        valorSillas = valorSillas - (freeTicketsWithPoints * discountSilla);
                    }
                    this.compraBoletas(allValidchair, idClient);
                }
                ViewBag.client = (from Clientes in db.Client
                                   where Clientes.Id == idClient
                                  select new ClientEntity
                                   {
                                       id = (int)Clientes.Id,
                                       nombre = Clientes.Name,
                                       identificacion = Clientes.Identification,
                                       totalPoints = Clientes.TotalPoints
                                   }).FirstOrDefault();
                ViewBag.totalPrice = valorSillas;
                ViewBag.idClient = idClient;
                ViewBag.isValidsChairs = isValidsChairs;
                ViewBag.isNotValidsChairs = isNotValidsChairs;
                ViewBag.arrayChairSelected = arrayChairSelected;
                ViewBag.idMovieByRoom = idMovieByRoom;
                ViewBag.ReturnDate = System.DateTime.Now;
                ViewBag.allData = this.getAllData(idMovieByRoom);
                ViewBag.isValid = isValid;
                ViewBag.puntosCliente = puntosCliente;
                ViewBag.freeTicketsWithPoints = freeTicketsWithPoints;
                return View("buyWindowTickets");
            }
        }

        private int getFreeTickets(int idClient)
        {
            using (var db = new CinemonitoEntities())
            {
                int freeticket = 0;
                var request = db.Client.Single(c => c.Id == idClient);

                while (request.TotalPoints >= 100)
                {
                    freeticket = freeticket + 1;
                    request.TotalPoints = request.TotalPoints - 100;
                }

                db.SaveChanges();
                return freeticket;
            }
        }

        private bool addPoints(int puntosCliente, int idClient)
        {
            using (var db = new CinemonitoEntities())
            {
                var request = db.Client.Single(c => c.Id == idClient);
                request.TotalPoints = request.TotalPoints + puntosCliente;
                db.SaveChanges();
                return true;
            }
        }

        private bool compraBoletas(List<int> allValidchair, int idCliente)
        {
            using (var db = new CinemonitoEntities())
            {
                foreach (int ids in allValidchair)
                {
                    var request = db.ChairByMovie.Single(c => c.Id == ids);
                    ChairEntity chairAllData = this.getOneChair((int)request.IdMovieByRoom, ids);
                    var addticket = new Ticket
                    {
                        IdClient = idCliente,
                        IdMovieByRoom = request.IdMovieByRoom,
                        Quantity = 1,
                        IdChair = chairAllData.idChair,
                    };
                    db.Ticket.Add(addticket);
                    request.IsAvailable = false;
                }
                db.SaveChanges();
                return true;
            }
        }

        private ChairEntity getOneChair(int idMovieByRoom, int Id)
        {
            using (var db = new CinemonitoEntities())
            {
                return (from MoviesByRoom in db.MoviesByRoom
                        join ChairByMovie in db.ChairByMovie on MoviesByRoom.IdMovieByRoom equals ChairByMovie.IdMovieByRoom
                        join Chair in db.Chair on ChairByMovie.IdChair equals Chair.Id
                        join TypeChair in db.TypeChair on Chair.IdTypeChair equals TypeChair.Id
                        where ChairByMovie.IdMovieByRoom.Equals(idMovieByRoom)
                        && ChairByMovie.Id == Id
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
        private AllDataInforEntity getAllData(int idMovieByRoom)
        {
            using (var db = new CinemonitoEntities())
            {
                return (from MoviesByRoom in db.MoviesByRoom
                        join Room in db.Room on MoviesByRoom.IdRoom equals Room.Id
                        join Headquarters in db.Headquarters on Room.IdHeadquarter equals Headquarters.Id
                        where MoviesByRoom.IdMovieByRoom == idMovieByRoom
                        select new AllDataInforEntity
                        {
                            horary = MoviesByRoom.Horary,
                            nameSala = Room.Name,
                            nameMultiplex = Headquarters.Name,
                            addressMultiplex = Headquarters.Address
                        }).FirstOrDefault();
            }
        }

        
        public ActionResult userBuys()
        {
           return View();
        }

        [HttpPost]
        public ActionResult SearchClient(string idClient)
        {
            ViewBag.idClient = idClient;
            Int32 response = -1;
            using (CinemonitoEntities db = new CinemonitoEntities())
            {
                var data = (from Client in db.Client
                            where Client.Identification.Equals(idClient)
                            select new { Id = Client.Id }).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.idClient = data.Id;
                    //ViewBag.Message = "Cliente registrado";
                    return View("buyTickets", from Movie in db.Movie.ToList() select Movie);
                }
                else
                {
                    ViewBag.Message = "Cliente NO registrado";
                    return View("userBuys");
                }
            }
            
           
        }



    }
}