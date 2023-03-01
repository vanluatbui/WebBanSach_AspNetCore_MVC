using E_learning;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebBanSach.DTO;
using WebBanSach.Entity;
using WebBanSach.Extension_Method;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class BookStoreController : Controller
    {
        private ApplicationDbContext data;

        public BookStoreController(ApplicationDbContext data)
        {
            this.data = data;
        }

        // Ham lay n quyen sach moi 
        private List<Sach> Laysachmoi(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            
            return data.Sachs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index(int pageNumber, string? search)
        {
            List<Sach> products = data.Sachs.ToList();

            if (search != null)
            {
                List<Sach> sachs = data.Sachs.ToList();

                products = new List<Sach>();

                //Tìm kiếm không dấu và không phân biệt hoa thường
                foreach (var item in sachs)
                {

                    if (CoDauSangKhongDau.LocDau(item.TenSach.ToLower()).
                        Contains(CoDauSangKhongDau.LocDau(search.ToLower())) == true)
                    {
                        products.Add(item);
                    }
                }
                 
                if (products.Count > 0)
                    ViewBag.KetQuaTimKiem = "Kết Quả Tìm Kiếm Cho : " + search;
                else
                    products = data.Sachs.ToList();
            }

            // Lấy tổng số dòng dữ liệu
            var totalItems = products.Count();

            int ITEMS_PER_PAGE = 10;

            // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
            List<Sach> pros = products.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
            ViewBag.TrangHienTai = pageNumber;
            ViewBag.TongSoTrang = totalPages;
            ViewBag.SetLink = "/BookStore/Index?pageNumber=";

            var taikhoan = HttpContext.Session.GetObject<ApplicationUser>("Taikhoan");

            if (taikhoan != null)
                ViewBag.TaiKhoan = taikhoan;

            return View(pros);
        }

        public ActionResult Sachtheochude(Guid id, int pageNumber)
        {
            List<Sach> products = data.Sachs.Where(p => p.MaChuDe == id).ToList();

            // Lấy tổng số dòng dữ liệu
            var totalItems = products.Count();

            int ITEMS_PER_PAGE = 10;

            // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
            List<Sach> pros = products.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
            ViewBag.TrangHienTai = pageNumber;
            ViewBag.TongSoTrang = totalPages;
            ViewBag.SetLink = "/BookStore/Sachtheochude?id="+id+"&&pageNumber=";
            return View(pros);
        }

        public ActionResult Chude()
        {
            var chude = data.ChuDes.ToList();
            return PartialView(chude);
        }

        public ActionResult Nhaxuatban()
        {
            var nhaxuatban = data.NhaXuatBans.ToList();
            return PartialView(nhaxuatban);
        }

        public ActionResult SachtheoNXB(Guid id, int pageNumber)
        {
            List<Sach> products = data.Sachs.Where(p => p.MaNXB == id).ToList();

            // Lấy tổng số dòng dữ liệu
            var totalItems = products.Count();

            int ITEMS_PER_PAGE = 10;

            // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
            List<Sach> pros = products.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
            ViewBag.TrangHienTai = pageNumber;
            ViewBag.TongSoTrang = totalPages;
            ViewBag.SetLink = "/BookStore/SachtheoNXB?id="+id+"&&pageNumber=";
            return View(pros);
        }

        public ActionResult Chitietsach(Guid id)
        {
            var sach = data.Sachs.Where(p => p.MaSach == id).FirstOrDefault();
            return View(sach);
        }
    }
}
