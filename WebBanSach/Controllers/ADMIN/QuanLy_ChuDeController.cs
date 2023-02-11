using Microsoft.AspNetCore.Mvc;
using WebBanSach.Extension_Method;
using WebBanSach.Models.Admin;
using WebBanSach.Models;
using E_learning;
using System.Collections.Generic;
using System.Xml.Linq;
using WebBanSach.Entity;

namespace WebBanSach.Controllers.ADMIN
{
    public class QuanLy_ChuDeController : Controller
    {
        private ApplicationDbContext data;

        public QuanLy_ChuDeController(ApplicationDbContext data)
        {
            this.data = data;
        }

        public ActionResult Chude(int pageNumber, string? search)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                List<ChuDe> chude = data.ChuDes.ToList();

                if (search != null)
                {
                    List<ChuDe> CDs = data.ChuDes.ToList();

                    chude = new List<ChuDe>();

                    //Tìm kiếm không dấu và không phân biệt hoa thường
                    foreach (var item in CDs)
                    {

                        if (CoDauSangKhongDau.LocDau(item.TenChuDe.ToLower()).
                            Contains(CoDauSangKhongDau.LocDau(search.ToLower())) == true)
                        {
                            chude.Add(item);
                        }
                    }

                    if (chude.Count > 0)
                        ViewBag.KetQuaTimKiem = "Kết Quả Tìm Kiếm Cho : " + search;
                    else
                        chude = data.ChuDes.ToList();
                }

                // Lấy tổng số dòng dữ liệu
                var totalItems = chude.Count();

                int ITEMS_PER_PAGE = 10;

                // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
                int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
                // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
                List<ChuDe> pros = chude.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
                ViewBag.TrangHienTai = pageNumber;
                ViewBag.TongSoTrang = totalPages;
                ViewBag.SetLink = "/QuanLy_ChuDe/Chude?pageNumber=";

                var ad = HttpContext.Session.GetObject<Admin>("Taikhoanadmin");

                if (ad != null)
                    ViewBag.TaiKhoanAdmin = ad;

                return View(pros);
            }
        }

        [HttpGet]
        public ActionResult Xoachude(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var cd = from s in data.ChuDes where s.MaChuDe == id select s;
                return View(cd.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Xoachude")]
        public ActionResult Xacnhanxoa_ChuDe(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ChuDe cd = data.ChuDes.SingleOrDefault(n => n.MaChuDe == id);

                data.ChuDes.Remove(cd);

                data.SaveChanges();
                return RedirectToAction("Chude", "QuanLy_ChuDe");
            }
        }

        [HttpGet]
        public ActionResult Themmoichude()
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ThemmoiChude(ChuDe cd)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {

                data.ChuDes.Add(cd);
                data.SaveChanges();

            }
            return RedirectToAction("Chude", "QuanLy_ChuDe");
        }

        //5 Điều chỉnh thông tin Chủ đề
        public ActionResult Suachude(Guid id)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ChuDe cd = data.ChuDes.SingleOrDefault(n => n.MaChuDe == id);
                return View(cd);
            }
        }

        private void UpdateModel_ChuDe(ChuDe cd)
        {
            //Luu vao CSDL
            ChuDe chude = data.ChuDes.FirstOrDefault(p => p.MaChuDe == cd.MaChuDe);
            chude.TenChuDe = cd.TenChuDe;

            data.ChuDes.Update(chude);
            data.SaveChanges();
        }

        [HttpPost, ActionName("Suachude")]
        public ActionResult Xacnhansua_ChuDe(ChuDe cd)
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                UpdateModel_ChuDe(cd);
                return RedirectToAction("Chude", "QuanLy_ChuDe");
            }
        }
    }
}
