using Microsoft.AspNetCore.Identity;

namespace WebBanSach.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set;}
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }
        public List<DonDatHang> listDonDatHang { get; set; }
    }
}
