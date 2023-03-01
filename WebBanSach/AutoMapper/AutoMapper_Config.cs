using AutoMapper;
using WebBanSach.DTO;
using WebBanSach.Models;

namespace WebBanSach.AutoMapper
{
    public class AutoMapper_Config : Profile
    {
        public AutoMapper_Config()
        {
            this.CreateMap<ChiTiet_DonDatHang, ChiTiet_DonDatHangDTO>();
            this.CreateMap<ChuDe, ChuDeDTO>();
            this.CreateMap<DonDatHang, DonDatHangDTO>();
            this.CreateMap<NhaXuatBan, NhaXuatBanDTO>();
            this.CreateMap<Sach, SachDTO>();
            this.CreateMap<TacGia, TacGiaDTO>();
            this.CreateMap<VietSach, VietSachDTO>();

            this.CreateMap<UserDTO, ApplicationUser>().ForMember(des => des.PasswordHash, act => act.MapFrom(src => src.Password));


            this.CreateMap<ChiTiet_DonDatHangDTO, ChiTiet_DonDatHang>();
            this.CreateMap<ChuDeDTO, ChuDe>();
            this.CreateMap<DonDatHangDTO, DonDatHang>();
            this.CreateMap<NhaXuatBanDTO, NhaXuatBan>();
            this.CreateMap<SachDTO, Sach>();
            this.CreateMap<TacGiaDTO, TacGia>();
            this.CreateMap<VietSachDTO, VietSach>();

            this.CreateMap<ApplicationUser, UserDTO>().ForMember(des => des.Password, act => act.MapFrom(src => src.PasswordHash));
        }
    }

}
