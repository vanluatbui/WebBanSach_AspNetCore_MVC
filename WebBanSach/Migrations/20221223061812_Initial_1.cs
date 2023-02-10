using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanSach.Migrations
{
    public partial class Initial_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KhachHang_Email_TaiKhoan",
                table: "KhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Email",
                table: "KhachHang",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_TaiKhoan",
                table: "KhachHang",
                column: "TaiKhoan",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KhachHang_Email",
                table: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_KhachHang_TaiKhoan",
                table: "KhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_Email_TaiKhoan",
                table: "KhachHang",
                columns: new[] { "Email", "TaiKhoan" },
                unique: true);
        }
    }
}
