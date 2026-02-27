using Timescale.Api.Application.DTOs;

namespace Timescale.Api.Application.Interfaces;

public interface IResultService
{
    Task<IEnumerable<ResultResponseDTO>> GetFilteredResultsAsync(ResultFilterDTO filter);
}