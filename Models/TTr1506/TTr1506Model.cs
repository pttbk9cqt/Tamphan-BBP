namespace Tamphan_BBP.Models.TTr1506
{
    public class TTr1506Model
    {
        public string TieuDe { get; set; } = "";
        public string SoToTrinh { get; set; } = "";
        public string NgayToTrinh { get; set; } = "";
        public string ThangToTrinh { get; set; } = "";
        public string NamToTrinh { get; set; } = "";

        public string DuAn { get; set; } = "";
        public string CongTrinh { get; set; } = "";
        public string HangMuc { get; set; } = "";
        public string DiaDiem { get; set; } = "";

        // composition Tâm thêm để gọi đến class SupplierInfo.cs
        public SupplierInfo Supplierinfo { get; set; } = new();

        public string CanCu { get; set; } = "";
        public string NguyenNhanTrinhKy { get; set; } = "";

    }
}
