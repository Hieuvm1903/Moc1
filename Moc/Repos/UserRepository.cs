using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Moc.Data;
using Moc.DTO;
using Moc.Models;

namespace Moc.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext ac;
        private string secretKey;
        public UserRepository(ApplicationContext ac, IConfiguration configuration)
        {
            this.ac = ac;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUnique(string username)
        {
            var user = ac.LocalUsers.FirstOrDefault(x => x.UserName == username);
            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = ac.LocalUsers.FirstOrDefault(s => s.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role),
                    
                   
                   // new Claim("Pass",user.Password)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return loginResponseDTO;



        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Password = registerationRequestDTO.Password,
                Name = registerationRequestDTO.Name,
                Role = registerationRequestDTO.Role
            };
            ac.LocalUsers.Add(user);
            await ac.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
