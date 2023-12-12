using Moc.DTO;
using Moc.Models;

namespace Moc.Repos
{
    public interface IUserRepository
    {
        bool IsUnique(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
