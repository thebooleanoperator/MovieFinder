using MovieFinder.DtoModels;
using MovieFinder.Models;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IIdentityService
    {
        Task<AuthenticationDto> RegisterUserAsync(CreateAccountDto createAccountDto);
        Task<AuthenticationDto> LoginAsync(LoginDto loginDto);
    }
}
