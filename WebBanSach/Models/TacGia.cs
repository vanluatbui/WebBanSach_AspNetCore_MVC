

using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("TacGia")]
    public class TacGia
    {
        public Guid MaTacGia { get; set; }
        public string TenTacGia { get; set; }
        public string DiaChi { get; set; }
        public string TieuSu { get; set; }
        public string DienThoai { get; set; }

        public List<VietSach> listVietSach { get; set; }
    }
}
