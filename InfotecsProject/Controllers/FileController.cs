using Microsoft.AspNetCore.Mvc;
using Timescale.Api.Application.Interfaces;

namespace Timescale.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран или пуст.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Допускаются только CSV файлы.");

        try
        {
            using var stream = file.OpenReadStream();
            await _fileService.ProcessCsvAsync(stream, file.FileName);
            return Ok(new { message = $"Файл {file.FileName} успешно обработан." });
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}