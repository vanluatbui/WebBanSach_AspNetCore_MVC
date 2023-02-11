using E_learning;
using Microsoft.AspNetCore.Mvc;
using WebBanSach.Extension_Method;
using WebBanSach.Models.Admin;
using WebBanSach.Models;
using WebBanSach.Entity;

namespace WebBanSach.Controllers.ADMIN
{
    public class QuanLy_NhaXuatBanController : Controller
    {
        private ApplicationDbContext data;
        public QuanLy_NhaXuatBanController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public ActionResult Nhaxuatban(int pageNumber, string? search)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                List<NhaXuatBan> listNXB = data.NhaXuatBans.ToList();

                if (search != null)
                {
                    List<NhaXuatBan> NXBs = data.NhaXuatBans.ToList();

                    listNXB = new List<NhaXuatBan>();

                    //Tìm kiếm không dấu và không phân biệt hoa thường
                    foreach (var item in NXBs)
                    {

                        if (CoDauSangKhongDau.LocDau(item.TenNXB.ToLower()).
                            Contains(CoDauSangKhongDau.LocDau(search.ToLower())) == true)
                        {
                            listNXB.Add(item);
                        }
                    }

                    if (listNXB.Count > 0)
                        ViewBag.KetQuaTimKiem = "Kết Quả Tìm Kiếm Cho : " + search;
                    else
                        listNXB = data.NhaXuatBans.ToList();
                }

                // Lấy tổng số dòng dữ liệu
                var totalItems = listNXB.Count();

                int ITEMS_PER_PAGE = 10;

                // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
                int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
                // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
                List<NhaXuatBan> pros = listNXB.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
                ViewBag.TrangHienTai = pageNumber;
                ViewBag.TongSoTrang = totalPages;
                ViewBag.SetLink = "/QuanLy_NhaXuatBan/Nhaxuatban?pageNumber=";

                var ad = HttpContext.Session.GetObject<Admin>("Taikhoanadmin");

                if (ad != null)
                    ViewBag.TaiKhoanAdmin = ad;

                return View(pros);
            }
        }

        [HttpGet]
        public ActionResult Xoanhaxuatban(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nxb = from s in data.NhaXuatBans where s.MaNXB == id select s;
                return View(nxb.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoanhaxuatban")]
        public ActionResult Xacnhanxoa_NXB(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                NhaXuatBan nxb = data.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);

                data.NhaXuatBans.Remove(nxb);

                data.SaveChanges();
                return RedirectToAction("Nhaxuatban", "QuanLy_NhaXuatBan");
            }
        }

        [HttpGet]
        public ActionResult Themmoinhaxuatban()
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ThemmoiNhaxuatban(NhaXuatBan nxb)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {

                data.NhaXuatBans.Add(nxb);
                data.SaveChanges();

            }
            return RedirectToAction("Nhaxuatban", "QuanLy_NhaXuatBan");
        }



        //5 Điều chỉnh thông tin Nhà xuất bản
        public ActionResult Suanhaxuatban(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                NhaXuatBan nxb = data.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);
                return View(nxb);
            }
        }

        private void UpdateModel_NXB(NhaXuatBan nxb)
        {
            // Cập nhật thông tin khác cho NXB...
            NhaXuatBan nhaxuatban = data.NhaXuatBans.FirstOrDefault(p => p.MaNXB == nxb.MaNXB);
            nhaxuatban.TenNXB = nxb.TenNXB;
            nhaxuatban.DiaChi = nxb.DiaChi;
            nhaxuatban.DienThoai = nxb.DienThoai;

            //Luu vao CSDL
            data.NhaXuatBans.Update(nhaxuatban);
            data.SaveChanges();
        }

        [HttpPost, ActionName("Suanhaxuatban")]
        public ActionResult Xacnhansua_NXB(NhaXuatBan nxb)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                UpdateModel_NXB(nxb);
                return RedirectToAction("Nhaxuatban", "QuanLy_NhaXuatBan");
            }
        }
    }
}
