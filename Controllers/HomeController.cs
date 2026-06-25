using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tamphan_BBP.Datalist;
using Tamphan_BBP.Models;

namespace Tamphan_BBP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Tâm thêm vào, ban đầu chỗ này chỉ có return View(); nhưng mình muốn truyền dữ liệu từ DatalistDuanCongtrinh sang View nên thêm 2 dòng dưới đây
            ViewBag.DuAns = DatalistDuanCongtrinh.DuAn;
            ViewBag.CongTrinhs = DatalistDuanCongtrinh.CongTrinh;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
