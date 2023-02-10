using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("Sach")]
    public class Sach
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
        public List<VietSach> listVietSach { get; set; }
        public List<ChiTiet_DonDatHang> listChiTiet_DDH { get; set; }
    }
}
