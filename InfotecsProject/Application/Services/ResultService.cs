using Microsoft.EntityFrameworkCore;
using Timescale.Api.Application.DTOs;
using Timescale.Api.Application.Interfaces;
using Timescale.Api.Infrastructure;

namespace Timescale.Api.Application.Services;

public class ResultService : IResultService
{
    private readonly AppDbContext _context;
    
    public ResultService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ResultResponseDTO>> GetFilteredResultsAsync(ResultFilterDTO filter)
    {
        var query = _context.Results
            .Include(r => r.File)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FileName))
        {
            query = query.Where(r => r.File.Name.Contains(filter.FileName));
        }
        
        if (filter.StartDateFrom.HasValue)
            query = query.Where(r => r.FirstOperationRun >= filter.StartDateFrom.Value);

        if (filter.StartDateTo.HasValue)
            query = query.Where(r => r.FirstOperationRun <= filter.StartDateTo.Value);

        if (filter.MinAverageValue.HasValue)
            query = query.Where(r => r.AverageValue >= filter.MinAverageValue.Value);

        if (filter.MaxAverageValue.HasValue)
            query = query.Where(r => r.AverageValue <= filter.MaxAverageValue.Value);

        if (filter.MinAverageExecutionTime.HasValue)
            query = query.Where(r => r.AverageExecutionTime >= filter.MinAverageExecutionTime.Value);

        if (filter.MaxAverageExecutionTime.HasValue)
            query = query.Where(r => r.AverageExecutionTime <= filter.MaxAverageExecutionTime.Value);

        return await query
            .Select(r => new ResultResponseDTO(
                r.File.Name,
                r.TimeDelta,
                r.FirstOperationRun,
                r.AverageExecutionTime,
                r.AverageValue,
                r.MedianValue,
                r.MaxValue,
                r.MinValue
            ))
            .ToListAsync();
    }
}