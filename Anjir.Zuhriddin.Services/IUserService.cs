using Anjir.Zuhriddin.Items.Services.ViewModels;

namespace Anjir.Zuhriddin.Items.Services
{
    public interface IUserService
    {
        Task<UserResultViewModel> LoginAsync(LoginUserViewModel model);
        Task<UserResultViewModel> RegistrationAsync(RegistrationUserViewModel model);
    }
}
