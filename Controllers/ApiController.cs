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
            //根據分類編號讀取景點資料
            var spots=_search.CategoryId==0? _dbContext.SpotImagesSpots:_dbContext.SpotImagesSpots.Where(s=>s.CategoryId==_search.CategoryId);
            //根據關鍵字查詢
            if (!string.IsNullOrEmpty(_search.Keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_search.Keyword) || s.SpotDescription.Contains(_search.Keyword));
            }
            //排序
            switch (_search.SortBy)//預設是空字串@Dto
            {
                case "SpotTitle":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = _search.SortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending(s => s.CategoryId);
                    break;
                default:
                    spots=_search.SortType =="asc"?spots.OrderBy(s=>s.SpotId):spots.OrderByDescending(s=>s.SpotId);
                    break;
            }
            //分頁
            int totalCount=spots.Count();//總共幾筆資料
            int pageSize=_search.PageSize??9;//每頁多少資料
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);//總頁數
            int page = _search.Page ?? 1;

            //取出分頁資料
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);

            //設計回傳資料格式
            SpotsPagingDto spotspaging=new SpotsPagingDto();
            spotspaging.TotalPages= totalPages;
            spotspaging.SpotsResult=spots.ToList();

            return Json(spotspaging);
            //return Json(spots);
        }
    }
}
