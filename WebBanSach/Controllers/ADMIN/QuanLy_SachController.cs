using E_learning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanSach.Entity;
using WebBanSach.Extension_Method;
using WebBanSach.Models;

namespace WebBanSach.Controllers.ADMIN
{
    public class QuanLy_SachController : Controller
    {
        private ApplicationDbContext data;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuanLy_SachController(ApplicationDbContext data, IWebHostEnvironment webHostEnvironment)
        {
            this.data = data;
            _webHostEnvironment = webHostEnvironment;
        }

        /////THực hiện các chức năng quản lý cho Table Sách
        //1. Hiện thị danh sách các nhà xuất bản
        public ActionResult Sach(int pageNumber, string? search)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                List<Sach> listSach = data.Sachs.ToList();

                if (search != null)
                {
                    List<Sach> sachs = data.Sachs.ToList();

                    listSach = new List<Sach>();

                    //Tìm kiếm không dấu và không phân biệt hoa thường
                    foreach (var item in sachs)
                    {

                        if (CoDauSangKhongDau.LocDau(item.TenSach.ToLower()).
                            Contains(CoDauSangKhongDau.LocDau(search.ToLower())) == true)
                        {
                            listSach.Add(item);
                        }
                    }

                    if (listSach.Count> 0)
                    ViewBag.KetQuaTimKiem = "Kết Quả Tìm Kiếm Cho : " + search;
                    else
                        listSach = data.Sachs.ToList();
                }

                // Lấy tổng số dòng dữ liệu
                var totalItems = listSach.Count();

                int ITEMS_PER_PAGE = 10;

                // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục do bạn cấu hình = 10, 20 ...)
                int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
                // Lấy phần tử trong  hang hiện tại (pageNumber là trang hiện tại - thường Binding từ route)
                List<Sach> pros = listSach.Skip(ITEMS_PER_PAGE * (pageNumber - 1)).Take(ITEMS_PER_PAGE).ToList();
                ViewBag.TrangHienTai = pageNumber;
                ViewBag.TongSoTrang = totalPages;
                ViewBag.SetLink = "/QuanLy_Sach/Sach?pageNumber=";

                var ad = HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin");

                if (ad != null)
                    ViewBag.TaiKhoanAdmin = ad;

                return View(pros);
            }
        }

        //2. Xem chi tiết sách
        public ActionResult Chitietsach(Guid id)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from s in data.Sachs where s.MaSach == id select s;
                return View(sach.SingleOrDefault());
            }
        }
        //3. Xóa 1 quyển sach: Hiện thị trang thông tin chi tiết sản phẩm cần xóa, sau đó xác nhận xóa.
        [HttpGet]
        public ActionResult Xoasach(Guid id)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from s in data.Sachs where s.MaSach == id select s;
                return View(sach.SingleOrDefault());
            }
        }

        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa_Sach(Guid id)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Sach sach = data.Sachs.SingleOrDefault(n => n.MaSach == id);

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Hinhsanpham", sach.AnhBia);
                FileInfo f = new FileInfo(path);
                f.Delete();

                data.Sachs.Remove(sach);

                data.SaveChanges();
                return RedirectToAction("Sach", "QuanLy_Sach");
            }
        }

        //4. Thêm mới 1 quyển Sách: Hiện thị view để thê mới, sau đó Lưu 
        [HttpGet]
        public ActionResult Themmoisach()
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.MaCD = new SelectList(data.ChuDes.ToList().OrderBy(n => n.TenChuDe).ToList(), "MaChuDe", "TenChuDe");
                ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB).ToList(), "MaNXB", "TenNXB");
                return View();
            }
        }

        [HttpPost]
        public ActionResult ThemmoiSach(Sach sach, IFormFile fileUpload, IFormCollection f)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //Kiem tra duong dan file
                if (fileUpload == null || fileUpload != null && fileUpload.FileName.Length == 0)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    ViewBag.MaCD = new SelectList(data.ChuDes.ToList().OrderBy(n => n.TenChuDe).ToList(), "MaChuDe", "TenChuDe");
                    ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB).ToList(), "MaNXB", "TenNXB");
                    return View();
                }
                //Them vao CSDL
                else
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Hinhsanpham", fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        using (Stream fileStream = new FileStream(path, FileMode.Create))
                        {
                            fileUpload.CopyTo(fileStream);
                        }
                        sach.AnhBia = fileName;

                        sach.MaChuDe = Guid.Parse(f["MaCD"].ToString());
                        sach.MaNXB = Guid.Parse(f["MaNXB"].ToString());
                        //Luu vao CSDL
                        data.Sachs.Add(sach);
                        data.SaveChanges();
                    }
                }
                return RedirectToAction("Sach", "QuanLy_Sach");
            }
        }

        //5 Điều chỉnh thông tin Sách
        public ActionResult Suasach(Guid id)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Sach sach = data.Sachs.SingleOrDefault(n => n.MaSach == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.MaCD = new SelectList(data.ChuDes.ToList().OrderBy(n => n.TenChuDe).ToList(), "MaChuDe", "TenChuDe", sach.MaChuDe);
                ViewBag.MaNXB = new SelectList(data.NhaXuatBans.ToList().OrderBy(n => n.TenNXB).ToList(), "MaNXB", "TenNXB", sach.MaNXB);
                return View(sach);
            }
        }

        private void UpdateModel_Sach(Sach sach, IFormCollection fo, IFormFile Anhbia)
        {
            // Tìm tên file ảnh bìa cũ để xoá

            Sach s = data.Sachs.FirstOrDefault(p => p.MaSach == sach.MaSach);

            if (Anhbia != null && Anhbia.FileName.Length > 0)
            {
                // Xoá ảnh cũ ...

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Hinhsanpham", s.AnhBia);
                FileInfo f = new FileInfo(path);
                f.Delete();

                // Cài thêm ảnh mới khác...

                var fileName = Path.GetFileName(Anhbia.FileName);
                var path2 = Path.Combine(_webHostEnvironment.WebRootPath, "Hinhsanpham", fileName);
                using (Stream fileStream = new FileStream(path2, FileMode.Create))
                {
                    Anhbia.CopyTo(fileStream);
                }
                s.AnhBia = fileName;
            }

            // Cập nhật thông tin khác cho sách...
            s.TenSach = sach.TenSach;
            s.NgayCapNhat = sach.NgayCapNhat;
            s.GiaBan = sach.GiaBan;
            s.MoTa = sach.MoTa;
            s.SoLuongTon = sach.SoLuongTon;

            s.MaChuDe = Guid.Parse(fo["MaCD"].ToString());
            s.MaNXB = Guid.Parse(fo["MaNXB"].ToString());

            //Luu vao CSDL
            data.Sachs.Update(s);
            data.SaveChanges();
        }

        [HttpPost, ActionName("Suasach")]
        public ActionResult Xacnhansua_Sach(Sach sach, IFormCollection f, IFormFile Anhbia)
        {
            if (HttpContext.Session.GetObject<ApplicationUser>("Taikhoanadmin") == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                UpdateModel_Sach(sach, f, Anhbia);
                return RedirectToAction("Sach", "QuanLy_Sach");
            }
        }
    }
}
