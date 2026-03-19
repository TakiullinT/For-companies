using Core.Entities;
using Core.Ports;
using Core.ResultInfo;
using Lab5.WebAPI.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.WebAPI.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionController : ControllerBase
{
    private readonly ICreateUserSession _createUserSession;
    private readonly ICreateAdminSession _createAdminSession;

    public SessionController(ICreateUserSession createUserSession, ICreateAdminSession createAdminSession)
    {
        _createUserSession = createUserSession;
        _createAdminSession = createAdminSession;
    }

    [HttpPost("user")]
    public IActionResult CreateUserSession([FromBody] UserSessionDto sessionDto)
    {
        ResultType<UserSession> result = _createUserSession.Execute(sessionDto.AccountId, sessionDto.Pin);
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("ПИН", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Unauthorized();
            }

            return BadRequest(result.ErrorMessage);
        }

        if (result.Value is not null)
        {
            return Ok(new { sessionKey = result.Value.SessionKey });
        }

        return BadRequest();
    }

    [HttpPost("admin")]
    public IActionResult CreateAdminSession([FromBody] AdminSessionDto dto)
    {
        ResultType<UserSession> result = _createAdminSession.Execute(dto.Password);
        if (!result.IsSuccess) return Unauthorized();
        if (result.Value != null)
        {
            return Ok(new { sessionKey = result.Value.SessionKey });
        }

        return BadRequest();
    }
}