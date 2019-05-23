using Cinemonito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinemonito.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
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


        public ActionResult movies()
        {
            using (var db = new CinemonitoEntities())
            {
                var data = db.Movie.ToList();
                return View(data);
            }
        }


    }
}