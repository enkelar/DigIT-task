using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public bool IsSold { get; set; }
}