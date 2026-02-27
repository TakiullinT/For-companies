using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Timescale.Api.Application.DTOs;
using Timescale.Api.Application.Interfaces;
using Timescale.Api.Domain.Entities;
using Timescale.Api.Domain.Exceptions;
using Timescale.Api.Infrastructure;

namespace Timescale.Api.Application.Services;

public class FileService : IFileService
{
    private readonly AppDbContext _context;

    public FileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ProcessCsvAsync(Stream fileStream, string fileName)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // В логе ошибки видно, что разделитель — точка с запятой
            Delimiter = ";", 
            HasHeaderRecord = true,
            PrepareHeaderForMatch = args => args.Header.Trim(' ', '"').ToLower(),
            // Это отключит строгую проверку заголовков (уберет ошибку про Id и FileId)
            HeaderValidated = null, 
            MissingFieldFound = null,
            ShouldSkipRecord = args => args.Row.Parser.Record.All(string.IsNullOrWhiteSpace)
        };
        
        using var reader = new StreamReader(fileStream, System.Text.Encoding.UTF8);
        using var csv = new CsvReader(reader, config);

        // Явное указание маппинга: берем только те поля, что есть в CSV
        csv.Context.RegisterClassMap<ValueEntityMap>();

        List<ValueEntity> records;
        try
        {
            records = csv.GetRecords<ValueEntity>().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CSV Error: {ex.Message}");
            throw new InvalidFileException($"Ошибка при чтении CSV: {ex.Message}");
        }

        if (records.Count < 1 || records.Count > 10000)
        {
            throw new InvalidFileException("Файл должен содержать от 1 до 10 000 строк.");
        }
        
        ValidateRecords(records);

        var fileEntity = new FileEntity { Name = fileName, UploadedAt = DateTime.UtcNow };
        _context.Files.Add(fileEntity);
        await _context.SaveChangesAsync();

        foreach (var record in records)
        {
            record.FileId = fileEntity.Id;
        }
        _context.Values.AddRange(records);

        var result = CalculateResults(records);
        result.FileId = fileEntity.Id;
        _context.Results.Add(result);

        await _context.SaveChangesAsync();
    }

    public sealed class ValueEntityMap : ClassMap<ValueEntity>
    {
        public ValueEntityMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.ExecutionTime).Name("ExecutionTime");
            Map(m => m.Value).Name("Value");
            
            Map(m => m.Id).Ignore();
            Map(m => m.FileId).Ignore();
            Map(m => m.File).Ignore();
        }
    }
    private void ValidateRecords(List<ValueEntity> records)
    {
        var now = DateTime.UtcNow.AddHours(1); // Небольшой запас для разных часовых поясов
        var minDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        foreach (var record in records)
        {
            if (record.Date > now || record.Date < minDate)
                throw new InvalidFileException($"Некорректная дата в файле: {record.Date}");

            if (record.ExecutionTime < 0)
                throw new InvalidFileException($"ExecutionTime не может быть отрицательным: {record.ExecutionTime}");

            if (record.Value < 0)
                throw new InvalidFileException($"Value не может быть отрицательным: {record.Value}");
        }
    }
    
    private ResultEntity CalculateResults(List<ValueEntity> records)
    {
        var dates = records.Select(r => r.Date).ToList();
        var executionTimes = records.Select(r => r.ExecutionTime).ToList();
        var values = records.Select(r => r.Value).OrderBy(v => v).ToList();

        return new ResultEntity
        {
            TimeDelta = (dates.Max() - dates.Min()).TotalSeconds,
            FirstOperationRun = dates.Min(),
            AverageExecutionTime = executionTimes.Average(),
            AverageValue = values.Average(),
            MaxValue = values.Max(),
            MinValue = values.Min(),
            MedianValue = CalculateMedian(values)
        };
    }
    
    private double CalculateMedian(List<double> sortedValues)
    {
        int count = sortedValues.Count;
        if (count == 0) return 0;

        if (count % 2 == 0)
        {
            return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0;
        }
        
        return sortedValues[count / 2];
    }
    
    public async Task<IEnumerable<ValueResponseDTO>> GetTenLastValuesAsync(string fileName)
    {
        var file = await _context.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Name == fileName);

        if (file == null) return Enumerable.Empty<ValueResponseDTO>();

        return await _context.Values
            .AsNoTracking()
            .Where(v => v.FileId == file.Id)
            .OrderByDescending(v => v.Date)
            .Take(10)
            .Select(v => new ValueResponseDTO
            {
                Date = v.Date,
                ExecutionTime = v.ExecutionTime,
                Value = v.Value
            })
            .ToListAsync();
    }
}