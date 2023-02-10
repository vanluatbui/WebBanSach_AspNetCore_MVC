using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        public Guid MaKH { get; set; }
        public string HoTen { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }

        public string Email { get; set; }
        public DateTime NgaySinh { get; set; }

        public List<DonDatHang> listDonDatHang { get; set; }
    }
}
