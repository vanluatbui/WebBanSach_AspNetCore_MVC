using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("DonDatHang")]
    public class DonDatHang
    {
        public Guid MaDonHang { get; set; }
        public byte TinhTrangGiaoHang { get; set; }
        public byte TinhTrangThanhToan { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime NgayGiao { get; set; }
        public Guid MaKH { get; set; }
        public KhachHang KhachHang { get; set; }

        public List<ChiTiet_DonDatHang> listChiTiet_DDH { get; set; }
    }
}
