using System.ComponentModel.DataAnnotations.Schema;
using WebBanSach.Models;

namespace WebBanSach.DTO
{
    public class ChiTiet_DonDatHangDTO
    {
        public Guid MaDonHang { get; set; }
        public Guid MaSach { get; set; }
        public DonDatHang DonDatHang { get; set; }
        public Sach Sach { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
    }
}
