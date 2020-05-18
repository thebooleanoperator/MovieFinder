using MovieFinder.DtoModels;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IIdentityService
    {
        Task<AuthenticationDto> RegisterUserAsync(CreateAccountDto createAccountDto);
        Task<AuthenticationDto> LoginAsync(LoginDto loginDto);
        Task<AuthenticationDto> RefreshTokenAsync(RefreshTokenRequestDto refreshRequest);
    }
}
