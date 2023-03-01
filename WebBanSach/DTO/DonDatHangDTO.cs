using System.ComponentModel.DataAnnotations.Schema;
using WebBanSach.Models;

namespace WebBanSach.DTO
{
    public class DonDatHangDTO
    {
        public Guid MaDonHang { get; set; }
        public byte TinhTrangGiaoHang { get; set; }
        public byte TinhTrangThanhToan { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime NgayGiao { get; set; }
        public string MaKH { get; set; }
        public ApplicationUser KhachHang { get; set; }
    }
}
