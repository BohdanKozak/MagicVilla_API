using MagicVilla_VillaAPI.Models.Dto.User_DTO;
using MagicVilla_Web.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDto);
    }
}
