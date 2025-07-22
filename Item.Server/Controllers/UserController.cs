using Anjir.Zuhriddin.Items.Services;
using Anjir.Zuhriddin.Items.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anjir.Zuhriddin.Items.Web.Controllers;
[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet("Login")]
    public async Task<LoginResult> LoginAsync([FromQuery] LoginUserViewModel model)
    {
        var user = await _userService.LoginAsync(model);
        if (user == null)
            throw new Exception("user not found");
        var res = _tokenService.CreateToken(user.Email);
        return new LoginResult()
        {
            AccessToken = res
        };
    }

    [HttpPost("Registration")]
    public async Task<UserResultViewModel> RegistrationAsync(RegistrationUserViewModel model)
    {
        return await _userService.RegistrationAsync(model);
    }
}
