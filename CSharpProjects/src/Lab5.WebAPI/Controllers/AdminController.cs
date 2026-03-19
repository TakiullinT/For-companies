using Core.Entities;
using Core.Ports;
using Core.ResultInfo;
using Lab5.WebAPI.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ICreateAccountService _createAccountService;

    public AdminController(ICreateAccountService createAccountService)
    {
        _createAccountService = createAccountService;
    }

    [HttpPost("createAccount")]
    public IActionResult CreateAccount([FromHeader] Guid sessionKey, [FromBody] CreateAccountDto createAccountDto)
    {
        ResultType<Account> result = _createAccountService.Execute(sessionKey, createAccountDto.Pin);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("Нет прав администратора", StringComparison.OrdinalIgnoreCase) == true ||
                result.ErrorMessage?.Contains("Сессия", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Unauthorized();
            }

            return BadRequest(result.ErrorMessage);
        }

        if (result.Value is not null) return Ok(new { accountId = result.Value.Id });
        return BadRequest();
    }
}