using DigitalPortfolio.API.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace DigitalPortfolio.API.Services.Interfaces;

public interface IAccountService
{
    public Task<IActionResult> LoginAsync(LoginModel loginModel);
    public Task<IActionResult> RegisterAsync(RegisterModel registerModel);
    public Task<IActionResult> GetUserInfoAsync(string username);
    public Task<IActionResult> SelfEditAsync(SelfEditModel selfEditModel, string username);
}