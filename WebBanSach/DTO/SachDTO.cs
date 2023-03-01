using System.ComponentModel.DataAnnotations.Schema;
using WebBanSach.Models;

namespace WebBanSach.DTO
{
    public class SachDTO
    {
        public Guid MaSach { get; set; }
        public string TenSach { get; set; }
        public double GiaBan { get; set; }
        public string MoTa { get; set; }
        public string AnhBia { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public int SoLuongTon { get; set; }
        public Guid MaChuDe { get; set; }
        public ChuDe ChuDe { get; set; }
        public Guid MaNXB { get; set; }
        public NhaXuatBan NhaXuatBan { get; set; }
    }
}
