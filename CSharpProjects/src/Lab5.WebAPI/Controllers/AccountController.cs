using Core.Entities;
using Core.Ports;
using Core.ResultInfo;
using Lab5.WebAPI.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lab5.WebAPI.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountController : ControllerBase
{
    private readonly IWithdrawMoneyService _withdrawMoneyService;
    private readonly IDepositMoneyService _depositMoneyService;
    private readonly IGetMoneyBalanceService _getMoneyBalanceService;
    private readonly IGetOperationHistoryService _getOperationsHistoryService;

    public AccountController(
        IWithdrawMoneyService withdrawMoneyService,
        IDepositMoneyService depositMoneyService,
        IGetMoneyBalanceService getMoneyBalanceService,
        IGetOperationHistoryService getOperationsHistoryService)
    {
        _withdrawMoneyService = withdrawMoneyService;
        _depositMoneyService = depositMoneyService;
        _getMoneyBalanceService = getMoneyBalanceService;
        _getOperationsHistoryService = getOperationsHistoryService;
    }

    [HttpPost("{accountId}/withdraw")]
    public ActionResult Withdraw(Guid accountId, [FromBody] MoneyDto amount, [FromHeader] Guid sessionKey)
    {
        Result result = _withdrawMoneyService.Execute(sessionKey, accountId, new MoneyState(amount.Value));
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("Нет доступа", StringComparison.OrdinalIgnoreCase) == true || result.ErrorMessage?.Contains("Сессия", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Unauthorized();
            }

            return BadRequest(result.ErrorMessage);
        }

        return Ok();
    }

    [HttpPost("{accountId}/deposit")]
    public ActionResult Deposit(Guid accountId, [FromBody] MoneyDto amount, [FromHeader] Guid sessionKey)
    {
        Result result = _depositMoneyService.Execute(sessionKey, accountId, new MoneyState(amount.Value));
        if (!result.IsSuccess)
        {
            if (result.ErrorMessage?.Contains("Нет доступа", StringComparison.OrdinalIgnoreCase) == true ||
                result.ErrorMessage?.Contains("Сессия", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Unauthorized();
            }

            return BadRequest(result.ErrorMessage);
        }

        return Ok();
    }

    [HttpGet("{accountId}/balance")]
    public ActionResult GetBalance(Guid accountId, [FromHeader] Guid sessionKey)
    {
        ResultType<MoneyState> balance = _getMoneyBalanceService.Execute(accountId, sessionKey);
        if (!balance.IsSuccess)
        {
            if (balance.ErrorMessage?.Contains("Нет доступа", StringComparison.OrdinalIgnoreCase) == true ||
                balance.ErrorMessage?.Contains("Сессия", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Unauthorized();
            }

            return BadRequest(balance.ErrorMessage);
        }

        return Ok(balance.Value);
    }

    [HttpGet("{accountId}/operationsHistory")]
    public ActionResult GetOperationsHistory(Guid accountId, [FromHeader] Guid sessionKey)
    {
        ResultType<IReadOnlyCollection<Operation>> operationsHistory =
                _getOperationsHistoryService.Execute(accountId, sessionKey);
        if (!operationsHistory.IsSuccess)
        {
            if (operationsHistory.ErrorMessage?.Contains("Нет доступа", StringComparison.OrdinalIgnoreCase) == true ||
                operationsHistory.ErrorMessage?.Contains("Сессия", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Unauthorized();
            }

            return BadRequest(operationsHistory.ErrorMessage);
        }

        return Ok(operationsHistory.Value);
    }
}