using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("ChiTiet_DonDatHang")]
    public class ChiTiet_DonDatHang
    {
        public Guid MaDonHang { get; set; }
        public Guid MaSach { get; set; }
        public DonDatHang DonDatHang { get; set; }
        public Sach Sach { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
    }
}
