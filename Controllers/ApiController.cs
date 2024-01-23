﻿using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ApiController : Controller
    {
        //注入DBContext物件--------------
        private readonly MyDBContext _dbContext;
        public ApiController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //-----------------------------------

        public IActionResult Index()
        {
            System.Threading.Thread.Sleep(5000);
            return Content("<h2>Content, 你好</h2>", "text/plain", System.Text.Encoding.UTF8);
          //  return View();
        }
        public IActionResult First()
        {
            return View();
        }

        public IActionResult Cities()
        {
           var cities = _dbContext.Addresses.Select(a => a.City).Distinct();
            return Json(cities);

        }

        public IActionResult Districts(string city)
        {
            var districts = _dbContext.Addresses.Where(a=>a.City == city).Select(c=>c.SiteId).Distinct();
            return Json(districts);

        }

        public IActionResult Avatar(int id=1)
        {
            Member? member =_dbContext.Members.Find(id);
            if (member != null) 
            {
                byte[] img = member.FileData;
                return File(img, "image/jpeg");
            }

            return NotFound();

        }

        public IActionResult Register(string name, int age=26)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Guest";
            }
            return Content($"Hello{name},you are{age} years old.");

        }
    }
}
