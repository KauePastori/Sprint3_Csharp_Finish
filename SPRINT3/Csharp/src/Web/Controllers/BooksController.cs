using Application.Files;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    private readonly IFileService _files;

    public BooksController(IBookService service, IFileService files)
    {
        _service = service;
        _files = files;
    }

    [HttpGet]
    public Task<List<Book>> GetAll(CancellationToken ct) => _service.GetAllAsync(ct);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> Get(int id, CancellationToken ct)
    {
        var book = await _service.GetAsync(id, ct);
        return book is null ? NotFound() : Ok(book);
    }

    public record CreateBookDto(string Title, string Author, decimal Price, DateTime PublishedOn);

    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] CreateBookDto dto, CancellationToken ct)
    {
        var b = await _service.CreateAsync(dto.Title, dto.Author, dto.Price, dto.PublishedOn, ct);
        await _files.AppendAuditLogAsync($"Created book {b.Id}");
        return CreatedAtAction(nameof(Get), new { id = b.Id }, b);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Book>> Update(int id, [FromBody] CreateBookDto dto, CancellationToken ct)
    {
        var b = await _service.UpdateAsync(id, dto.Title, dto.Author, dto.Price, dto.PublishedOn, ct);
        await _files.AppendAuditLogAsync($"Updated book {b.Id}");
        return Ok(b);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        await _files.AppendAuditLogAsync($"Deleted book {id}");
        return NoContent();
    }

    [HttpPost("import/json")]
    public async Task<ActionResult<int>> ImportJson([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("Empty file.");
        using var stream = file.OpenReadStream();
        var count = await _files.ImportBooksFromJsonAsync(stream, ct);
        return Ok(count);
    }

    [HttpGet("export/json")]
    public async Task<FileResult> ExportJson(CancellationToken ct)
    {
        var bytes = await _files.ExportBooksToJsonAsync(ct);
        return File(bytes, "application/json", "books.json");
    }

    [HttpGet("export/txt")]
    public async Task<FileResult> ExportTxt(CancellationToken ct)
    {
        var bytes = await _files.ExportBooksToTxtAsync(ct);
        return File(bytes, "text/plain", "books.txt");
    }
}