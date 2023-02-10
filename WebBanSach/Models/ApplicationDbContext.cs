
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Emit;
using WebBanSach.Models;
using WebBanSach.Models.Admin;

namespace E_learning
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<ChuDe> ChuDes { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<TacGia> TacGias { get; set; }

        public DbSet<VietSach> VietSachs { get; set; }

        public DbSet<DonDatHang> DonDatHangs { get; set; }
        public DbSet<ChiTiet_DonDatHang> ChiTiet_DonDatHangs { get; set; }

        //ADMIN

        public DbSet<Admin> Admins { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Table KhachHang :

            builder.Entity<KhachHang>(entity =>
            {
                // PK
                entity.HasKey(p => p.MaKH);

                // Setting For Any Properties...

                entity.Property(p => p.HoTen).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
                entity.Property(p => p.TaiKhoan).HasColumnType("varchar").HasMaxLength(50);
                entity.Property(p => p.MatKhau).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
                entity.Property(p => p.DiaChi).HasColumnType("nvarchar").HasMaxLength(200);
                entity.Property(p => p.DienThoai).HasColumnType("varchar").HasMaxLength(50);
                entity.Property(p => p.Email).HasColumnType("varchar").HasMaxLength(100);

                entity.HasIndex(p => new { p.Email }).IsUnique();
                entity.HasIndex(p => new { p.TaiKhoan }).IsUnique();
            });

            // Table chuDe :

            builder.Entity<ChuDe>(entity =>
            {
                // PK
                entity.HasKey(p => p.MaChuDe);

                // Setting For Any Properties...

                entity.Property(p => p.TenChuDe).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            });

            // Table NhaXuatBan :

            builder.Entity<NhaXuatBan>(entity =>
            {
                // PK
                entity.HasKey(p => p.MaNXB);

                // Setting For Any Properties...

                entity.Property(p => p.TenNXB).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
                entity.Property(p => p.DiaChi).HasColumnType("nvarchar").HasMaxLength(200);
                entity.Property(p => p.DienThoai).HasColumnType("varchar").HasMaxLength(50);
            });

            // Table Sach :

            builder.Entity<Sach>(entity =>
            {
                // PK
                entity.HasKey(p => p.MaSach);

                // Setting For Any Properties...

                entity.Property(p => p.TenSach).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
                entity.Property(p => p.MoTa).HasColumnType("nvarchar").HasMaxLength(500).IsRequired(true);
                entity.Property(p => p.AnhBia).HasColumnType("varchar").HasMaxLength(50);
                entity.HasOne<ChuDe>(p => p.ChuDe).WithMany(q => q.listSach).HasForeignKey(s => s.MaChuDe);
                entity.HasOne<NhaXuatBan>(p => p.NhaXuatBan).WithMany(q => q.listSach).HasForeignKey(s => s.MaNXB);
            });

            // Table TacGia :

            builder.Entity<TacGia>(entity =>
            {
                // PK
                entity.HasKey(p => p.MaTacGia);

                // Setting For Any Properties...

                entity.Property(p => p.TenTacGia).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
                entity.Property(p => p.DiaChi).HasColumnType("nvarchar").HasMaxLength(100);
                entity.Property(p => p.TieuSu).HasColumnType("nvarchar").HasMaxLength(500);
                entity.Property(p => p.DienThoai).HasColumnType("varchar").HasMaxLength(50);
            });

            // Table VietSach :

            builder.Entity<VietSach>(entity =>
            {
                // PK
                entity.HasKey(p => new { p.MaTacGia, p.MaSach });

                // Setting For Any Properties...

                entity.Property(p => p.VaiTro).HasColumnType("nvarchar").HasMaxLength(50);
                entity.Property(p => p.ViTri).HasColumnType("nvarchar").HasMaxLength(50);
                entity.HasOne<TacGia>(p => p.TacGia).WithMany(q => q.listVietSach).HasForeignKey(s => s.MaTacGia);
                entity.HasOne<Sach>(p => p.Sach).WithMany(q => q.listVietSach).HasForeignKey(s => s.MaSach);
            });

            // Table DonDatHang :

            builder.Entity<DonDatHang>(entity =>
            {
                // PK
                entity.HasKey(p => p.MaDonHang);

                // Setting For Any Properties...

                entity.HasOne<KhachHang>(p => p.KhachHang).WithMany(q => q.listDonDatHang).HasForeignKey(s => s.MaKH);
            });

            // Table VietSach :

            builder.Entity<ChiTiet_DonDatHang>(entity =>
            {
                // PK
                entity.HasKey(p => new { p.MaDonHang, p.MaSach });

                // Setting For Any Properties...

                entity.HasOne<DonDatHang>(p => p.DonDatHang).WithMany(q => q.listChiTiet_DDH).HasForeignKey(s => s.MaDonHang);
                entity.HasOne<Sach>(p => p.Sach).WithMany(q => q.listChiTiet_DDH).HasForeignKey(s => s.MaSach);
            });

            // Table ADMIN :

            builder.Entity<Admin>(entity =>
            {
                // PK
                entity.HasKey(p => p.userAdmin);

                // Setting For Any Properties...

                entity.Property(p => p.userAdmin).HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true); ;
                entity.Property(p => p.passAdmin).HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
                entity.Property(p => p.HoTen).HasColumnType("nvarchar").HasMaxLength(100);
            });
        }
    }
}
