using AutoMapper;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.DTOs.Auth;
using MiddleAssignment.Backend.DTOs.Login;
using MiddleAssignment.Backend.DTOs.Register;
using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<ReviewDTO, Review>().ReverseMap();
            CreateMap<BookBorrowingRequestDto, BookBorrowingRequest>().ReverseMap();
            CreateMap<BookBorrowingRequestDetail, BookBorrowingRequestDetailDto>().ReverseMap();

            CreateMap<User, RegistrationResponse>();
            CreateMap<AuthenticationRequest, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            CreateMap<User, AuthenticationResponse>()
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());

            CreateMap<LoginRequest, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Username));

            CreateMap<User, LoginResponse>()
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
        }
    }
}
