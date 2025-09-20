using Domain.Entities;

namespace Application.Files;

public interface IFileService
{
    Task<int> ImportBooksFromJsonAsync(Stream jsonStream, CancellationToken ct = default);
    Task<byte[]> ExportBooksToJsonAsync(CancellationToken ct = default);
    Task<byte[]> ExportBooksToTxtAsync(CancellationToken ct = default);
    Task AppendAuditLogAsync(string message, CancellationToken ct = default);
}