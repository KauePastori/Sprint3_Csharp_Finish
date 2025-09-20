using Domain.Entities;

namespace Application.Services;

public interface IBookService
{
    Task<Book> CreateAsync(string title, string author, decimal price, DateTime publishedOn, CancellationToken ct = default);
    Task<List<Book>> GetAllAsync(CancellationToken ct = default);
    Task<Book?> GetAsync(int id, CancellationToken ct = default);
    Task<Book> UpdateAsync(int id, string title, string author, decimal price, DateTime publishedOn, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}