using Microsoft.AspNetCore.Mvc;
using Tamphan_BBP.Datalist;
using Tamphan_BBP.Models.TTr1506;
using Tamphan_BBP.Services;

namespace Tamphan_BBP.Controllers
{
    public class TTr1506Controller : Controller
    {
        private readonly LoadContentService _loadContentService;
        public TTr1506Controller(LoadContentService loadContentService)
        {
            _loadContentService = loadContentService;
        }
        public IActionResult Index()
        {
            ViewBag.DuAns = DatalistDuanCongtrinh.DuAn;
            ViewBag.CongTrinhs = DatalistDuanCongtrinh.CongTrinh;

            var contents = _loadContentService.Load("PTTam_TTr1506_Content");
            ViewBag.Contents = contents;

            return View();
        }

        [HttpPost]
        public IActionResult Build(TTr1506Model model)
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
                            { "NHACUNGCAP", model.Supplierinfo.NhaCungCap },
                            { "DIACHI", model.Supplierinfo.Diachi },
                            { "SDT", model.Supplierinfo.SoDienThoai },
                            { "EMAIL", model.Supplierinfo.Email },
                            { "STK", model.Supplierinfo.STK },
                            { "MST", model.Supplierinfo.MST },
                            { "DAIDIEN", model.Supplierinfo.DaiDien },
                            { "CHUCVU", model.Supplierinfo.ChucVu },
                            { "CANCU", model.CanCu },
                            { "NGUYENNHANTRINHKY", model.NguyenNhanTrinhKy }
                        };


            var downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            Directory.CreateDirectory(downloadFolder);

            void BuildFile(string template, string outputName)
            {
                var outputPath = Path.Combine(downloadFolder, outputName);
                BuildWordService.Build(template, outputPath, data);
            }

            BuildFile(@"Templates\TTr1506\BM-15-06 TỜ TRÌNH XIN CHỦ TRƯƠNG - Templates.docx", "BM-15-06 TỜ TRÌNH XIN CHỦ TRƯƠNG.docx");

            //BuildFile(@"Templates\BTS\BM-15-07 TỜ TRÌNH KÝ BIÊN BẢN THỎA THUẬN - Templates.docx", "BM-15-07 TỜ TRÌNH KÝ BIÊN BẢN THỎA THUẬN.docx");

            //BuildFile(@"Templates\BTS\BIÊN BẢN THỎA THUẬN BTS - Templates.docx", "BIÊN BẢN THỎA THUẬN.docx");

            //BuildFile(@"Templates\BTS\BM-62-12 BIÊN BẢN BÀN GIAO MẶT BẰNG BTS - Templates.docx", "BM-62-12 BIÊN BẢN BÀN GIAO MẶT BẰNG BTS.docx");

            return Content("Đã tạo xong.");
            //return Json(model);
        }

    }
}
