using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<FileController> _logger;

    public FileController(ApplicationDbContext dbContext, ILogger<FileController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        try
        {
            // Logging informational message
            _logger.LogInformation("Received a swift file request.");

            using var streamReader = new StreamReader(file.OpenReadStream());
            string line;
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                var lineEntry = new FileEntry { Line = line };
                _dbContext.LineEntries.Add(lineEntry);
            }
            await _dbContext.SaveChangesAsync();

            var records = await _dbContext.LineEntries.ToListAsync();

            // Logging Sucess Message
            _logger.LogInformation("File uploaded successfully.");

            return Ok(records);
        }
        catch (Exception ex)
        {
            // Logging Error Message
            _logger.LogError(ex, "An error occurred.");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
            
        
    }
}
