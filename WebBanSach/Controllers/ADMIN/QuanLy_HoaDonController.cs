using E_learning;
using Microsoft.AspNetCore.Mvc;
using WebBanSach.Extension_Method;
using WebBanSach.Models.Admin;
using WebBanSach.Models;
using WebBanSach.Entity;

namespace WebBanSach.Controllers.ADMIN
{
    public class QuanLy_HoaDonController : Controller
    {
        private ApplicationDbContext data;

        public QuanLy_HoaDonController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public ActionResult Hoadon(int pageNumber, string? search)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                List<DonDatHang> listDDH = data.DonDatHangs.ToList();

                if (search != null)
                {
                    List<DonDatHang> sachs = data.DonDatHangs.ToList();

                    listDDH = new List<DonDatHang>();

                    //Tìm kiếm không dấu và không phân biệt hoa thường
                    foreach (var item in sachs)
                    {

                        if (CoDauSangKhongDau.LocDau(item.MaDonHang.ToString().ToLower()).
                            Contains(CoDauSangKhongDau.LocDau(search.ToLower())) == true)
                        {
                            listDDH.Add(item);
                        }
                    }

                    if (listDDH.Count > 0)
                        ViewBag.KetQuaTimKiem = "Kết Quả Tìm Kiếm Cho : " + search;
                    else
                        listDDH = data.DonDatHangs.ToList();
                }

                // Lấy tổng số dòng dữ liệu
                var totalItems = listDDH.Count();

                int ITEMS_PER_PAGE = 10;

                // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
                int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
                // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
                List<DonDatHang> pros = listDDH.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
                ViewBag.TrangHienTai = pageNumber;
                ViewBag.TongSoTrang = totalPages;
                ViewBag.SetLink = "/QuanLy_HoaDon/Hoadon?pageNumber=";
                return View(pros);
            }
        }

        [HttpGet]
        public ActionResult Xoahoadon(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                // Xoá hoá đơn thì đồng nghĩa xoá luôn các chi tiết trong hoá đơn?

                List<ChiTiet_DonDatHang> listCTHD= data.ChiTiet_DonDatHangs.Where(p => p.MaDonHang == id).ToList();
                foreach(var detail_HD in listCTHD)
                {
                    data.ChiTiet_DonDatHangs.Remove(detail_HD);
                }

                // Tiến hành xoá hoá đơn...

                DonDatHang hd = data.DonDatHangs.SingleOrDefault(n => n.MaDonHang == id);
                data.DonDatHangs.Remove(hd);

                data.SaveChanges();
                return RedirectToAction("Hoadon", "QuanLy_HoaDon");
            }
        }

        // Cập nhật tình trạng thanh toán của hoá dơn...
        public ActionResult CapNhatHoaDon_ThanhToan (Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DonDatHang hd = data.DonDatHangs.FirstOrDefault(p => p.MaDonHang == id);
                if (hd.TinhTrangThanhToan == 0)
                {
                    hd.TinhTrangThanhToan = 1;
                }
                else
                {
                    hd.TinhTrangThanhToan = 0;
                }
                data.DonDatHangs.Update(hd);
                data.SaveChanges();
                return RedirectToAction("Hoadon", "QuanLy_HoaDon");
            }
        }

        // Cập nhật tình trạng giao hàng của hoá dơn...
        public ActionResult CapNhatHoaDon_GiaoHang(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                DonDatHang hd = data.DonDatHangs.FirstOrDefault(p => p.MaDonHang == id);
                if (hd.TinhTrangGiaoHang == 0)
                {
                    hd.TinhTrangGiaoHang = 1;
                }
                else
                {
                    hd.TinhTrangGiaoHang = 0;
                }
                data.DonDatHangs.Update(hd);
                data.SaveChanges();
                return RedirectToAction("Hoadon", "QuanLy_HoaDon");
            }
        }

        //Xem chi tiết hoá đơn?
        public ActionResult Chitiethoadon(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                List<ChiTiet_DonDatHang> list_HoaDon = data.ChiTiet_DonDatHangs.Where(p => p.MaDonHang == id).ToList();
                return View(list_HoaDon);
            }
        }

        //Xem chi tiết sản phẩm trong hoá đơn
        public ActionResult Chitietsach(Guid id, string strURL)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from s in data.Sachs where s.MaSach == id select s;
                ViewBag.Link_Back = strURL;
                return View(sach.SingleOrDefault());
            }
        }
    }
}
