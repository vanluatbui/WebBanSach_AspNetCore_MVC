using E_learning;
using WebBanSach.Models;

namespace WebBanSach.Entity
{
    public class Giohang
    {
        public Guid iMasach { set; get; }
        public string sTensach { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
    }
}
