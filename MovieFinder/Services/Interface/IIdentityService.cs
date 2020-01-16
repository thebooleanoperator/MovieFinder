using MovieFinder.DtoModels;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IIdentityService
    {
        Task<AuthenticationDto> RegisterUserAsync(CreateAccountDto createAccountDto);
    }
}
