using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto.User_DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UsersController : Controller
    {


        private readonly IUserRepository _userRepo;
        protected APIResponce _responce;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _responce = new APIResponce();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {

                _responce.StatusCode = HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorMessages.Add("Username or password is incorrect ");

                return BadRequest(_responce);
            }
            _responce.StatusCode = HttpStatusCode.OK;
            _responce.IsSuccess = true;
            _responce.Result = loginResponse;
            return Ok(loginResponse);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {


            if (!_userRepo.IsUniqueUser(model.UserName))
            {
                _responce.StatusCode = HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorMessages.Add("Username is already exists ");
                return BadRequest(_responce);
            }

            var user = await _userRepo.Register(model);

            if (user == null)
            {
                _responce.StatusCode = HttpStatusCode.BadRequest;
                _responce.IsSuccess = false;
                _responce.ErrorMessages.Add("Error while registering");
                return BadRequest(_responce);
            }

            _responce.StatusCode = HttpStatusCode.OK;

            return Ok(_responce);
        }
    }
}
