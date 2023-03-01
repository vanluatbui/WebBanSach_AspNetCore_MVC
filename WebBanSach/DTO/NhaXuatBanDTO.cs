using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.DTO
{
    public class NhaXuatBanDTO
    {
        public Guid MaNXB { get; set; }
        public string TenNXB { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }

    }
}
