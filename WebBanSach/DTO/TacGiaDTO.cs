

using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.DTO
{
    public class TacGiaDTO
    {
        public Guid MaTacGia { get; set; }
        public string TenTacGia { get; set; }
        public string DiaChi { get; set; }
        public string TieuSu { get; set; }
        public string DienThoai { get; set; }
    }
}
