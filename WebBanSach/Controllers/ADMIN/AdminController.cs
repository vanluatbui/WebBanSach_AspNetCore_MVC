using E_learning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanSach.Entity;
using WebBanSach.Extension_Method;
using WebBanSach.Models;
using WebBanSach.Models.Admin;

namespace WebBanSach.Controllers.ADMIN
{
    public class AdminController : Controller
    {
        private ApplicationDbContext data;
        public AdminController(ApplicationDbContext data)
        {
            this.data = data;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var ad = HttpContext.Session.GetObject<Admin>("Taikhoanadmin");

                if (ad != null)
                    ViewBag.TaiKhoanAdmin = ad;

                return View();
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(IFormCollection f)
        {
            var tendn = f["txtuser"];
            var matkhau = f["txtpass"];
            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập ";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            else
            {
                var ad = data.Admins.SingleOrDefault(n => n.userAdmin == tendn.ToString() && n.passAdmin == MD5_Encrypt.MD5_Encrypt.MD5_Password(matkhau.ToString()));
                if (ad != null)
                {
                    HttpContext.Session.SetObject("Taikhoanadmin", ad);
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            }

            return View();
        }

        public ActionResult Logout()
        {
            if (HttpContext.Session.GetObject<Admin>("Taikhoanadmin") != null)
                HttpContext.Session.SetObject("Taikhoanadmin", null);

                return RedirectToAction("Index", "BookStore");
        }
    }
}
