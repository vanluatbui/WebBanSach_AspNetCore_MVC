using AutoMapper;
using E_learning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using WebBanSach.DTO;
using WebBanSach.Extension_Method;
using WebBanSach.Models;

namespace WebBanSach.Controllers.ADMIN
{
    public class AdminController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        private UserManager<ApplicationUser> userManager { get; set; }
        private RoleManager<IdentityRole> roleManager { get; set; }
        private IConfiguration configuration { get; set; }
        private IMapper mapper { get; set; }

        private ApplicationDbContext context { get; set; }
        public AdminController(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            _signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var ad = HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin");

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
        public async Task<ActionResult> Login(IFormCollection f)
        {
            var tendn = f["txtuser"];
            var matkhau = f["txtpass"];
            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập ";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            else
            {
                try
                {
                    var user = await userManager.FindByNameAsync(tendn);
            
                    if (user == null)
                    {
                        ViewBag.Thongbao = "Tên đăng nhập không tồn tại";
                        return View();
                    }
                    var result = await userManager.CheckPasswordAsync(user, matkhau);

                    if (result)
                    {
                        HttpContext.Session.SetObject("Taikhoanadmin", user);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        

                        return RedirectToAction("Index", "Admin");
                    }
                    else
                        ViewBag.Thongbao = "Mật khẩu tài khoản này không hợp lệ";
                }
                catch (Exception ex)
                {

                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") != null)
                HttpContext.Session.SetObject("Taikhoanadmin", null);
            
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            return RedirectToAction("Index", "BookStore");
        }

    }
}
