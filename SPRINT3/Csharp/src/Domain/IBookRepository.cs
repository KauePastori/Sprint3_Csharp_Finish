using Domain.Entities;

namespace Domain.Repositories;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Book>> GetAllAsync(CancellationToken ct = default);
    Task<Book> AddAsync(Book book, CancellationToken ct = default);
    Task UpdateAsync(Book book, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}