using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApi.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Range(0, 99999)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    public bool IsSold { get; set; }
}