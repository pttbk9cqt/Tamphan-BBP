using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Tamphan_BBP.Datalist;
using Tamphan_BBP.Models.BTS;
using Tamphan_BBP.Services;

namespace Tamphan_BBP.Controllers
{
    public class BTSController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.DuAns = DatalistDuanCongtrinh.DuAn;
            ViewBag.CongTrinhs = DatalistDuanCongtrinh.CongTrinh;
            return View();
        }

        [HttpPost]
        public IActionResult Build(BTSModel model)
        {
            try
            {
                var data = new Dictionary<string, string>
                        {
                            { "TIEUDE", model.TieuDe },
                            { "SOTTR", model.SoToTrinh },
                            { "DAY", model.NgayToTrinh.ToString() },
                            { "MONTH", model.ThangToTrinh.ToString() },
                            { "YEAR", model.NamToTrinh.ToString() },
                            { "DUAN", model.DuAn },
                            { "CONGTRINH", model.CongTrinh },
                            { "HANGMUC", model.HangMuc },
                            { "DIADIEM", model.DiaDiem },
                            { "NHACUNGCAP", model.NhaCungCap },
                            { "DIACHI", model.Diachi },
                            { "SDT", model.SoDienThoai },
                            { "EMAIL", model.Email },
                            { "STK", model.STK },
                            { "MST", model.MST },
                            { "DAIDIEN", model.DaiDien },
                            { "CHUCVU", model.ChucVu },
                            { "MATRAM", model.MaTram },
                            { "L11", model.L11 },
                            { "L12", model.L12 },
                            { "L21", model.L21 },
                            { "L22", model.L22 },
                            { "L31", model.L31 },
                            { "L32", model.L32 },
                            { "L41", model.L41 },
                            { "L42", model.L42 }
                        };


                var downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                Directory.CreateDirectory(downloadFolder);

                void BuildFile(string template, string outputName)
                {
                    var outputPath = Path.Combine(downloadFolder, outputName);
                    BuildWordService.Build(template, outputPath, data);
                }

                BuildFile(@"Templates\BTS\BM-15-06 TỜ TRÌNH XIN CHỦ TRƯƠNG BTS - Templates.docx", "BM-15-06 TỜ TRÌNH XIN CHỦ TRƯƠNG.docx");

                BuildFile(@"Templates\BTS\BM-15-07 TỜ TRÌNH KÝ BIÊN BẢN THỎA THUẬN BTS - Templates.docx", "BM-15-07 TỜ TRÌNH KÝ BIÊN BẢN THỎA THUẬN.docx");

                BuildFile(@"Templates\BTS\BIÊN BẢN THỎA THUẬN BTS - Templates.docx", "BIÊN BẢN THỎA THUẬN.docx");

                BuildFile(@"Templates\BTS\BM-62-12 BIÊN BẢN BÀN GIAO MẶT BẰNG BTS - Templates.docx", "BM-62-12 BIÊN BẢN BÀN GIAO MẶT BẰNG BTS.docx");

                return Content("Đã tạo xong.");
                //return Json(model);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
    }
}
