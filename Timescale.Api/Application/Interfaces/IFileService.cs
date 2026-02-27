using Timescale.Api.Application.DTOs;

namespace Timescale.Api.Application.Interfaces;

public interface IFileService
{
    Task ProcessCsvAsync(Stream fileStream, string fileName);
    Task<IEnumerable<ValueResponseDTO>> GetTenLastValuesAsync(string fileName);
}