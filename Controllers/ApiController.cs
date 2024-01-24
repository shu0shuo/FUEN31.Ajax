using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.Dtos;

namespace WebApplication2.Controllers
{
    public class ApiController : Controller
    {
        //注入DBContext物件--------------
        private readonly MyDBContext _dbContext;
        private readonly IWebHostEnvironment _host;
        public ApiController(MyDBContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
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

        public IActionResult Register(UserDto _user)
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
                _user.Name = "Guest";
            }
           //string uploadPath=@"C:/......."

            //todo 檔案重名
            //todo 限制檔案類型
            //todo 限制上傳大小
            string fileName = "enpty.jpeg";
            if(_user.Avatar!=null)
            {
                fileName = _user.Avatar.FileName;
            }

            //取得檔案實際路徑
            string uploadPath = Path.Combine(_host.WebRootPath, "uploads", fileName);

            using(var fileStream=new FileStream(uploadPath, FileMode.Create))
            {
                _user.Avatar?.CopyTo(fileStream);
            }

            return Content($"{_user.Avatar?.FileName}-{_user.Avatar?.Length}-{_user.Avatar?.ContentType}");

        }

        //return Content($"Hello{_user.Name},you are{_user.Age} years old. Yuor email address is {_user.Email}");


        //public IActionResult Register(string name, int age = 26)
        //{
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        name = "Guest";
        //    }
        //    return Content($"Hello{name},you are{age} years old.");

        //}

        [HttpPost]
        //frombody表示預告要從body傳資料進來
        public IActionResult Spots([FromBody]SearchDto _search) 
        {
            return Json(_search);
        }
    }
}
