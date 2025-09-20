using Application.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Application;

public class BookService : IBookService
{
    private readonly IBookRepository _repo;

    public BookService(IBookRepository repo) => _repo = repo;

    public async Task<Book> CreateAsync(string title, string author, decimal price, DateTime publishedOn, CancellationToken ct = default)
    {
        var book = new Book(title, author, price, publishedOn);
        return await _repo.AddAsync(book, ct);
    }

    public Task<List<Book>> GetAllAsync(CancellationToken ct = default) => _repo.GetAllAsync(ct);

    public Task<Book?> GetAsync(int id, CancellationToken ct = default) => _repo.GetByIdAsync(id, ct);

    public async Task<Book> UpdateAsync(int id, string title, string author, decimal price, DateTime publishedOn, CancellationToken ct = default)
    {
        var book = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException($"Book {id} not found");
        book.Update(title, author, price, publishedOn);
        await _repo.UpdateAsync(book, ct);
        return book;
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) => _repo.DeleteAsync(id, ct);
}