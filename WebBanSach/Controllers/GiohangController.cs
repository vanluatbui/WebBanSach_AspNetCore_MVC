using E_learning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanSach.Entity;
using WebBanSach.Models;
using WebBanSach.Extension_Method;
using System.Runtime.CompilerServices;

namespace Webbansach.Controllers
{
    public class GiohangController : Controller
    {

        private ApplicationDbContext data;

        public GiohangController(ApplicationDbContext data)
        {
            this.data = data;
        }

        //Lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = HttpContext.Session.GetObject<List<Giohang>>("Giohang");
            if (lstGiohang == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listGiohang
                lstGiohang = new List<Giohang>();
                //HttpContext.Session.SetObject("Giohang", lstGiohang);
            }
            return lstGiohang;
        }
        //Them hang vao gio
        public ActionResult ThemGiohang(Guid id, string strURL)
        {
            //Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sách này tồn tại trong Session["Giohang"] chưa?
            Giohang sanpham = lstGiohang.Find(n => n.iMasach == id);
            if (sanpham == null)
            {
                sanpham = new Giohang();

                //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1

                sanpham.iMasach = id;
                Sach sach = data.Sachs.FirstOrDefault(n => n.MaSach == id);
                sanpham.sTensach = sach.TenSach;
                sanpham.sAnhbia = sach.AnhBia;
                sanpham.dDongia = double.Parse(sach.GiaBan.ToString());
                sanpham.iSoluong = 1;

                lstGiohang.Add(sanpham);
                HttpContext.Session.SetObject("Giohang", lstGiohang);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                HttpContext.Session.SetObject("Giohang", lstGiohang);
                return Redirect(strURL);
            }
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = HttpContext.Session.GetObject<List<Giohang>>("Giohang");
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = HttpContext.Session.GetObject<List<Giohang>>("Giohang");
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        //HIen thi giỏ hàng.
        public ActionResult GioHang(int pageNumber)
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            if (lstGiohang.Count == 0)
                return RedirectToAction("Index", "BookStore");
            else
            {
                // Lấy tổng số dòng dữ liệu
                var totalItems = lstGiohang.Count();

                int ITEMS_PER_PAGE = 10;

                // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
                int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
                // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
                var  pros = lstGiohang.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
                ViewBag.TrangHienTai = pageNumber;
                ViewBag.TongSoTrang = totalPages;
                ViewBag.SetLink = "/Giohang/GioHang?pageNumber=";
                return View(pros);
            }
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            return PartialView();
        }
        //Xoa Giohang
        public ActionResult Xoagiohang(Guid id)
        {
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == id);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasach == id);
                HttpContext.Session.SetObject("Giohang", lstGiohang);
                return RedirectToAction("GioHang");

            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }
        //Xóa tất cả giỏ hàng
        public ActionResult Xoatatcagiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            HttpContext.Session.SetObject("Giohang", null);
            return RedirectToAction("Index", "BookStore");
        }
        //Cap nhat Giỏ hàng
        public ActionResult CapnhatGiohang(Guid id, IFormCollection f)
        {

            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasach == id);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
                HttpContext.Session.SetObject("Giohang", lstGiohang);
            }
            return RedirectToAction("GioHang","Giohang");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoan") == null || HttpContext.Session.GetObject<ApplicationUser>("Taikhoan").ToString() == "")
                return RedirectToAction("Dangnhap", "Nguoidung");
            if (HttpContext.Session.GetObject<List<Giohang>>("Giohang") == null)
                return RedirectToAction("Index", "BookStore");


            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            // Lấy thông tin khách hàng đang đặt hàng...

            ApplicationUser kh = HttpContext.Session.GetObject<ApplicationUser>("Taikhoan");
            ViewBag.HoTen = kh.FullName;
            ViewBag.DiaChi = kh.Address;
            ViewBag.DienThoai = kh.PhoneNumber;

            return View(lstGiohang);
        }
        [HttpPost]
        //Xay dung chuc nang Dathang
           public ActionResult DatHang(IFormCollection collection)
        {
            //Them Don hang
            DonDatHang ddh = new DonDatHang();
            ddh.MaDonHang = new Guid();
            ApplicationUser kh = HttpContext.Session.GetObject<ApplicationUser>("Taikhoan");
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.Id;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangThanhToan = 0;
            ddh.TinhTrangGiaoHang = 0;
            data.DonDatHangs.Add(ddh);
            data.SaveChanges();
            //Them chi tiet don hang            
            foreach (var item in gh)
            {
                ChiTiet_DonDatHang ctdh = new ChiTiet_DonDatHang();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMasach;
                ctdh.SoLuong = item.iSoluong;
                ctdh.DonGia = (double)item.dDongia * item.iSoluong;
                data.ChiTiet_DonDatHangs.Add(ctdh);
            }
            data.SaveChanges();
            HttpContext.Session.SetObject("Giohang", null);
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}