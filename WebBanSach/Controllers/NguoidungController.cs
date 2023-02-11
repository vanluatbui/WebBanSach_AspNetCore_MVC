using E_learning;
using Microsoft.AspNetCore.Mvc;
using WebBanSach.Models;
using WebBanSach.Extension_Method;
using NuGet.Common;
using WebBanSach.Send_Gmail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanSach.Models.Admin;

namespace WebBanSach.Controllers
{
    public class NguoidungController : Controller
    {
        private ApplicationDbContext db;

        public NguoidungController(ApplicationDbContext data)
        {
            this.db = data;
        }

        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        // POST: Hàm Dangky(post) Nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(IFormCollection collection, KhachHang kh)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }

            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập điện thoai";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                kh.MaKH = new Guid();
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = MD5_Encrypt.MD5_Encrypt.MD5_Password(matkhau);
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            return this.Dangky();
        }

        [HttpGet]
        public ActionResult ForgetPassWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassWord(IFormCollection f)
        {
            var confirm = f["ConfirmEmail"];
            if (String.IsNullOrEmpty(confirm))
            {
                ViewData["Loi1"] = "Email khôi phục không được để trống";
            }
            else if (db.KhachHangs.FirstOrDefault(p => p.Email == confirm.ToString()) == null)
            {
                ViewData["Loi1"] = "Email này chưa liên kết tài khoản nào";
            }
            else
            {
                Random r = new Random();
                var x = r.Next(12345, 98765);

                SendEmail.SendMail2Step("bvanluat2000@gmail.com",
             confirm.ToString(), "Web Ban Sach - Reset Password", "Your code to reset your password is " + x.ToString() + ". Please return and use this code to enter !", "pkzrigpkwkrffyet");
                TempData["Code"] = x.ToString();

                return RedirectToAction("ConfirmEmail", "Nguoidung", new { email = confirm });
            }
            return this.ForgetPassWord();
        }

        [HttpGet]
        public ActionResult ConfirmEmail(string email)
        {
            ViewBag.Email = email;
            ViewBag.Code = TempData["Code"];
            TempData["Code"] = ViewBag.Code;

            if (db.KhachHangs.FirstOrDefault(p => p.Email == email) == null)
            {
                ViewData["Loi1"] = "Email này chưa liên kết tài khoản nào";
                return this.ForgetPassWord();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmEmail(IFormCollection f, string email)
        {
            string codeResult = TempData["Code"].ToString();

            var confirm = f["EmailConfirm"];
            if (String.IsNullOrEmpty(confirm))
            {
                ViewData["Loi1"] = "Mã xác minh không được trống";
            }
            else if (String.Compare(confirm, codeResult, false) != 0)
            {
                ViewData["Loi1"] = "Mã xác minh không đúng";
            }
            else
            {
                return RedirectToAction("ChangePassword", "Nguoidung", new { email = email });
            }
            return this.ConfirmEmail(email);
        }

        [HttpGet]
        public ActionResult ChangePassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(IFormCollection f, string email)
        {
            var pass = f["Password"];
            var confirm = f["PasswordConfirm"];
            if (String.IsNullOrEmpty(pass))
            {
                ViewData["Loi1"] = "Trường này không được trống";
            }
            else if (String.IsNullOrEmpty(confirm))
            {
                ViewData["Loi2"] = "Trường này không được trống";
            }
            else if (String.Compare(confirm, pass, false) != 0)
            {
                ViewData["Loi2"] = "Mật khẩu xác nhận không trùng khớp";
            }
            else
            {
                KhachHang kh = db.KhachHangs.FirstOrDefault(p => p.Email == email);
                kh.MatKhau = MD5_Encrypt.MD5_Encrypt.MD5_Password(pass);
                db.KhachHangs.Update(kh);
                db.SaveChanges();

                return RedirectToAction("Dangnhap");
            }
            return this.ChangePassword(email);
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            var taikhoan = HttpContext.Session.GetObject<KhachHang>("Taikhoan");

            if (taikhoan != null)
                return RedirectToAction("Index","BookStore");

            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(IFormCollection f)
        {
            string tendn = f["TenDN"];
            string matkhau = MD5_Encrypt.MD5_Encrypt.MD5_Password(f["Matkhau"]);

            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            else
            {

                var kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    //Session["Taikhoan"] = kh;
                    HttpContext.Session.SetObject("Taikhoan", kh);

                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            }

            return View();
        }

        //--------------------------------------------------------------------------------------

        public ActionResult Thongtinnguoidung(Guid id)
        {
            if (HttpContext.Session.GetObject<KhachHang>("Taikhoan") == null)
                return RedirectToAction("Dangnhap", "Nguoidung");
            else
            {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);

                return View(kh);
            }
        }

        private void UpdateModel_KhachHang(KhachHang kh, IFormCollection f)
        {
            KhachHang s = db.KhachHangs.FirstOrDefault(p => p.MaKH == kh.MaKH);

            // Cập nhật thông tin khác cho khách hàng...
            s.TaiKhoan = kh.TaiKhoan;
            s.NgaySinh = kh.NgaySinh;
            s.DiaChi = kh.DiaChi;
            s.DienThoai = kh.DienThoai;
            s.Email = kh.Email;
            s.HoTen = kh.HoTen;

            //Luu vao CSDL
            db.KhachHangs.Update(s);
            db.SaveChanges();
        }

        [HttpPost, ActionName("Suakhachhang")]
        public ActionResult Xacnhansua_Nguoidung(KhachHang kh, IFormCollection f)
        {
            if (HttpContext.Session.GetObject<KhachHang>("Taikhoan") == null)
                return RedirectToAction("Dangnhap", "Nguoidung");
            else
            {
                UpdateModel_KhachHang(kh, f);
                HttpContext.Session.SetObject("Taikhoan", kh);
                return RedirectToAction("Index", "BookStore");
            }
        }

        public ActionResult Logout()
        {
            if (HttpContext.Session.GetObject<KhachHang>("Taikhoan") == null)
                return RedirectToAction("Dangnhap", "Nguoidung");
            else
            {
                HttpContext.Session.SetObject("Taikhoan", null);
                return RedirectToAction("Index", "BookStore");
            }
        }
    }
 }
