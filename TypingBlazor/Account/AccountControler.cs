using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace TypingBlazor.Account;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly UserManager<TypingUser> userManager;
    private readonly SignInManager<TypingUser> signInManager;


    public AccountController(UserManager<TypingUser> userManager, SignInManager<TypingUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        var user = new TypingUser
        {
            UserName = registerModel.UserName,
        };
        var result = await userManager.CreateAsync(user, registerModel.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return Ok(); 

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {

        var result = await signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }
        return Ok(); 
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok();

    }

    [Authorize]
    [HttpGet("currentuser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await userManager.GetUserAsync(User);
       
        UserDto userDto = new UserDto
        {
            UserName = user.UserName,
            TotalCharCount = user.TotalCharCount,
            CorrectCharCount = user.CorrectCharCount ,
            Accuracy = user.Accuracy ,
            TimeTrained = user.TimeTrained,
            LastAccuracy= user.StatisticsOfLastTraining?.Accuracy ?? 0,
            LastcharacterPerMinute = user.StatisticsOfLastTraining?.characterPerMinute ?? 0
        };
        return Ok(  userDto);
    }

    [Authorize]
    [HttpPost("updatestats")]
    public async Task<IActionResult> UpdateStats([FromBody] UpdateStatsModel statsModel)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        user.TotalCharCount = user.TotalCharCount  + statsModel.TotalCharCount;
        user.CorrectCharCount = user.CorrectCharCount  + statsModel.CorrectCharCount;
        user.TimeTrained = user.TimeTrained + TimeSpan.FromSeconds(statsModel.TimeTrainedInSeconds);
        user.StatisticsOfLastTraining = new StatisticsOfLastTraining
        {
            characterPerMinute = statsModel.LastCharacterPerMinute,
            Accuracy = statsModel.LastAccuracy
        };
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

  
}
