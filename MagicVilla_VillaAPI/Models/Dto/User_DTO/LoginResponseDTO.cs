using MagicVilla_Web.Models;

namespace MagicVilla_VillaAPI.Models.Dto.User_DTO
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
