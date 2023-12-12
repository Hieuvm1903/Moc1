using System.Net;
//using Fluent.Infrastructure.FluentStartup;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moc.DTO;
using Moc.Models;
using Moc.Repos;

namespace Moc.Controllers
{
    [ApiController]
    [Route("api/UsersAuth")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        protected APIResponse response;
        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            response = new APIResponse();
        }
        [HttpPost("login")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {

            var loginResponse = await userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("User or password is incorrect");
                return BadRequest(response);

            }
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = loginResponse;

            return Ok(response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool isUserNameUnique = userRepository.IsUnique(model.UserName);
            if (!isUserNameUnique)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Username already existed");
                return BadRequest(response);
            }
            var user = await userRepository.Register(model);
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Error while registering");
                return BadRequest(response);
            }
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return Ok(response);
        }
    }
}
