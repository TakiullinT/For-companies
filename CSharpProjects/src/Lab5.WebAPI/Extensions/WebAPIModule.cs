using Core.Ports;
using Core.Services;

namespace Lab5.WebAPI.Extensions;

public static class WebAPIModule
{
    public static IServiceCollection AddWebAPIModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICreateUserSession, CreateUserSession>();
        services.AddScoped<ICreateAdminSession, CreateAdminSession>();

        services.AddScoped<IWithdrawMoneyService, WithdrawMoneyService>();
        services.AddScoped<IDepositMoneyService, DepositMoneyService>();
        services.AddScoped<IGetMoneyBalanceService, GetMoneyBalanceService>();
        services.AddScoped<IGetOperationHistoryService, GetOperationsHistoryService>();

        services.AddScoped<ICreateAccountService, CreateAccountService>();

        return services;
    }
}