namespace Domain.Entities;

/// <summary>
/// Book entity represents a simple aggregate with basic validation encapsulated.
/// </summary>
public class Book
{
    public int Id { get; set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public DateTime PublishedOn { get; private set; }

    public Book(string title, string author, decimal price, DateTime publishedOn)
    {
        Update(title, author, price, publishedOn);
    }

    public void Update(string title, string author, decimal price, DateTime publishedOn)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required");
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Author is required");
        if (price < 0) throw new ArgumentException("Price must be positive");
        Title = title.Trim();
        Author = author.Trim();
        Price = price;
        PublishedOn = publishedOn;
    }
}