using System.ComponentModel.DataAnnotations.Schema;
using WebBanSach.Models;

namespace WebBanSach.DTO
{
    public class VietSachDTO
    {
        public Guid MaTacGia { get; set; }
        public Guid MaSach { get; set; }
        public TacGia TacGia { get; set; }
        public Sach Sach { get; set; }
        public string ViTri { get; set; }
        public string VaiTro { get; set; }
    }
}
