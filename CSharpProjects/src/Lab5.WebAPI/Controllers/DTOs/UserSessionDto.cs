namespace Lab5.WebAPI.Controllers.DTOs;

public class UserSessionDto
{
    public Guid AccountId { get; set; }

    public string Pin { get; set; } = string.Empty;
}