using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanSach.Models
{
    [Table("ChuDe")]
    public class ChuDe
    {
        public Guid MaChuDe { get; set; }
        public string TenChuDe { get; set; }
        public List<Sach> listSach { get; set; }
    }
}
