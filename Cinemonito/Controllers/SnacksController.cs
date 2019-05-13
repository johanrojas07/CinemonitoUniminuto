using Cinemonito.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cinemonito.Controllers
{
    public class SnacksController : Controller
    {


        List<DetailSaleProducts> DetailProductsSale = new List<DetailSaleProducts>();

        public SnacksController()
        {
            ViewBag.DetailProductsSale = DetailProductsSale;
        }

        public string IdClient { get; set; }

        public ActionResult buySnacks()
        {
            LoadProducts();
            return View();
        }

        [HttpPost]
        public ActionResult SearchClient(string idClient)
        {
            ViewBag.idClient = idClient;
            bool response = false;
            using (CinemonitoEntities db = new CinemonitoEntities())
            {
                response = db.Client.ToList().Where(c => c.Identification.Equals(idClient)).Any();
            }
            
            if(response)
            {               
                ViewBag.Message = "Cliente registrado";
            }
            else
            {
                ViewBag.Message = "Cliente NO registrado";
            }

            ViewBag.ClientRegister = response;
            LoadProducts();

            return View("buySnacks");
        }

        [HttpPost]
        public ActionResult AddProduct(string idClient)
        {
            DetailSaleProducts productSale = new DetailSaleProducts
            {
                IdProduct = 1,
                Quantity = 4
            };
            DetailProductsSale.Add(productSale);
            ViewBag.DetailProductsSale = DetailProductsSale;
            LoadProducts();
            return View("buySnacks");
        }

        [HttpPost]
        public ActionResult GetPrice(string id)
        {
            return View();
        }

        private void LoadProducts()
        {
            List<Product> Products = new List<Product>();
            using (CinemonitoEntities db = new CinemonitoEntities())
            {
                Products = db.Product.ToList();
            }
            ViewBag.Products = Products;
        }
    }
}