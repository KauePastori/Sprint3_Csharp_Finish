
namespace Sprint3WinForms;

/// <summary>Domain entity representing a Book.</summary>
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PublishedOn { get; set; }
}
