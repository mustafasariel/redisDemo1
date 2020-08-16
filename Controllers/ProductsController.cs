using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace redisDemo1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            this._distributedCache = distributedCache;
           
        }

        public IActionResult Index()
        {

            DistributedCacheEntryOptions distributedCacheOption = new DistributedCacheEntryOptions();
            distributedCacheOption.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
            _distributedCache.SetString("xyz", "100",distributedCacheOption);
            return View();
        }

        public IActionResult Show()
        {
            ViewBag.test = _distributedCache.GetString("xyz");
            return View();
        }
    }
}
