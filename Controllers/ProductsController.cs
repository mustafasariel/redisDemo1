using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using redisDemo1.Models;

namespace redisDemo1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;

        }

        //public IActionResult Index()
        //{

        //    DistributedCacheEntryOptions distributedCacheOption = new DistributedCacheEntryOptions();
        //    distributedCacheOption.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
        //  //  distributedCacheOption.SlidingExpiration = new TimeSpan(1000 * 60 * 5);

        //    _distributedCache.SetString("adim", "mustafa", distributedCacheOption);



        //    Product product1 = new Product { Id = 1, Name = "kalem", Price = 30 };

        //    var jsonProduct1 = JsonConvert.SerializeObject(product1);

        //    _distributedCache.SetString("Product:1", jsonProduct1, distributedCacheOption);

        //    return View();
        //}

        public async  Task<IActionResult> Index()
        {

            DistributedCacheEntryOptions distributedCacheOption = new DistributedCacheEntryOptions();
            distributedCacheOption.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
            //  distributedCacheOption.SlidingExpiration = new TimeSpan(1000 * 60 * 5);

            _distributedCache.SetString("adim", "mustafa", distributedCacheOption);



            Product product1 = new Product { Id = 1, Name = "kalem", Price = 30 };

            var jsonProduct1 = JsonConvert.SerializeObject(product1);

           await _distributedCache.SetStringAsync("Product:1", jsonProduct1, distributedCacheOption);

            Product product2 = new Product { Id = 2, Name = "defter", Price = 30 };

            var jsonProduct2 = JsonConvert.SerializeObject(product2);

            await _distributedCache.SetStringAsync("Product:2", jsonProduct2, distributedCacheOption);


            Product product3 = new Product { Id = 3, Name = "silgi", Price = 5 };

            Byte[] byteProduct = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(product3));

            _distributedCache.Set("Product:3", byteProduct);

            return View();
        }

        public IActionResult Show()
        {
            //    ViewBag.test = _distributedCache.GetString("xyz");
                ViewBag.adim = _distributedCache.GetString("adim");
            //    ViewBag.soyadim = _distributedCache.GetString("lastname");


            string jsonProduct1 = _distributedCache.GetString("Product:1");
            Product product1 = JsonConvert.DeserializeObject<Product>(jsonProduct1);
            ViewBag.Product1 = product1;


            string jsonProduct2 = _distributedCache.GetString("Product:2");
            Product product2 = JsonConvert.DeserializeObject<Product>(jsonProduct2);
            ViewBag.Product2 = product2;

            byte[] byteProduct3 = _distributedCache.Get("Product:3");

            Product product3 = JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(byteProduct3));

            ViewBag.Product3 = product3;


            return View();
        }

        public IActionResult ImageCache()
        {
            string strPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/m41.jpg");
            byte[] byteImage = System.IO.File.ReadAllBytes(strPath);

            _distributedCache.Set("resim", byteImage);
            return View();
        }

        public IActionResult ImageShow()
        {
            byte[] resimByte = _distributedCache.Get("resim");
            return File(resimByte, "image/jpg");

        }

    }
}
