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
            ViewBag.DuAns = DatalistDuanCongtrinh.DuAn;
            ViewBag.CongTrinhs = DatalistDuanCongtrinh.CongTrinh;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Totrinh1506()
        {
            ViewBag.DuAns = DatalistDuanCongtrinh.DuAn;
            ViewBag.CongTrinhs = DatalistDuanCongtrinh.CongTrinh;
            return View("1506");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
