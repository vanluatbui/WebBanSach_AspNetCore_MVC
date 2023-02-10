using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("NhaXuatBan")]
    public class NhaXuatBan
    {
        public Guid MaNXB { get; set; }
        public string TenNXB { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }

        public List<Sach> listSach { get; set; }
    }
}
