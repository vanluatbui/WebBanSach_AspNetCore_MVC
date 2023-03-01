using E_learning;
using Microsoft.AspNetCore.Mvc;
using WebBanSach.Models;
using WebBanSach.Extension_Method;
using NuGet.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebBanSach.DTO;
using WebBanSach.Send_Gmail;

namespace WebBanSach.Controllers
{
    public class NguoidungController : Controller
    {
        private ApplicationDbContext db;

        private UserManager<ApplicationUser> userManager { get; set; }
        private RoleManager<IdentityRole> roleManager { get; set; }
        private IConfiguration configuration { get; set; }
        private IMapper mapper { get; set; }

        public NguoidungController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ApplicationDbContext data)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.db = data;
        }

        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        // POST: Hàm Dangky(post) Nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public async Task<ActionResult> Dangky (IFormCollection collection)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = collection["HotenKH"].ToString();
            var tendn = collection["TenDN"].ToString();
            var matkhau = collection["Matkhau"].ToString();
            var matkhaunhaplai = collection["Matkhaunhaplai"].ToString();
            var diachi = collection["Diachi"].ToString();
            var email = collection["Email"].ToString();
            var dienthoai = collection["Dienthoai"].ToString();
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

                try
                {
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    if (!await roleManager.RoleExistsAsync("Guest"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Guest"));
                    }

                    //------------------------------------------------

                    ApplicationUser user = new ApplicationUser();
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = tendn.ToString();
                    user.Email = email.ToString();

                    user.PhoneNumber = dienthoai.ToString();
                    user.Address = diachi.ToString();
                    user.FullName = hoten.ToString();
                    user.BirthDate = DateTime.Parse(ngaysinh);

                    PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
                    user.PasswordHash = hasher.HashPassword(user, matkhau.ToString());


                    var result = await userManager.CreateAsync(user);

                    await userManager.AddToRoleAsync(user, "Guest");

                    return RedirectToAction("Dangnhap", "Nguoidung");
                }
                catch (Exception ex)
                {
                    
                }

            }
            return this.Dangky();
        }

        [HttpGet]
        public ActionResult ForgetPassWord()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassWord(IFormCollection f)
        {
            var confirm = f["ConfirmEmail"];
            if (String.IsNullOrEmpty(confirm))
            {
                ViewData["Loi1"] = "Email khôi phục không được để trống";
            }
            else if (await userManager.FindByEmailAsync(confirm) == null)
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
        public async Task<ActionResult> ConfirmEmail(string email)
        {
            ViewBag.Email = email;
            ViewBag.Code = TempData["Code"];
            TempData["Code"] = ViewBag.Code;

            if (await userManager.FindByEmailAsync(email) == null)
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
            return RedirectToAction("ConfirmEmail", "Nguoidung", new { email = email });
        }

        [HttpGet]
        public ActionResult ChangePassword(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(IFormCollection f, string email)
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
                var kh = await userManager.FindByEmailAsync(email);

                await userManager.RemovePasswordAsync(kh);
                PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
                kh.PasswordHash = hasher.HashPassword(kh, pass);

                await userManager.UpdateAsync(kh);

                return RedirectToAction("Dangnhap");
            }
            return this.ChangePassword(email);
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            var taikhoan = HttpContext.Session.GetObject<ApplicationUser>("Taikhoan");

            if (taikhoan != null)
                return RedirectToAction("Index","BookStore");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Dangnhap(IFormCollection f)
        {
            string tendn = f["TenDN"];
            string matkhau = f["Matkhau"];

            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            else
            {
                try
                {
                    var user = await userManager.FindByNameAsync(tendn.ToString());

                    if (user == null)
                    {
                        ViewBag.Thongbao = "Tên đăng nhập không tồn tại";
                        return View();
                    }
                    var result = await userManager.CheckPasswordAsync(user, matkhau.ToString());

                    if (result)
                    {
                        HttpContext.Session.SetObject("Taikhoan", user);
                        return RedirectToAction("Index", "BookStore");

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

        //--------------------------------------------------------------------------------------

        public async Task<ActionResult> Thongtinnguoidung (string id)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoan") == null)
                return RedirectToAction("Dangnhap", "Nguoidung");
            else
            {
                var kh = await userManager.FindByIdAsync(id);

                return View(kh);
            }
        }

        private async Task UpdateModel_KhachHang (ApplicationUser userModel)
        {
            try
            {
                var user = await userManager.FindByNameAsync(userModel.UserName);

                user.UserName = userModel.UserName; //nếu cần thay đổi Username khác
                user.Email = userModel.Email;
                user.FullName = userModel.FullName;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Address = userModel.Address;


                var result = await userManager.UpdateAsync(user);
            }
            catch
            {
                
            }
        }

        [HttpPost, ActionName("Suakhachhang")]
        public ActionResult Xacnhansua_Nguoidung(ApplicationUser kh, IFormCollection f)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoan") == null)
                return RedirectToAction("Dangnhap", "Nguoidung");
            else
            {
                UpdateModel_KhachHang(kh);
                HttpContext.Session.SetObject("Taikhoan", kh);
                return RedirectToAction("Index", "BookStore");
            }
        }

        public ActionResult Logout()
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoan") == null)
                return RedirectToAction("Dangnhap", "Nguoidung");
            else
            {
                HttpContext.Session.SetObject("Taikhoan", null);
                return RedirectToAction("Index", "BookStore");
            }
        }
    }
 }
