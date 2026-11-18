using BooksApi.Data;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
        var books = await _context.Books.ToListAsync();
        return Ok(books);
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return NotFound();

        return Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<Book>> Create(Book book)
    {
        if (await _context.Books.AnyAsync(b => b.Title == book.Title && b.Author == book.Author))
            return Conflict("A book with this title and author already exists.");
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT: api/books/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Book book)
    {
        if (id != book.Id)
            return BadRequest();

        if (await _context.Books.AnyAsync(b => b.Title == book.Title && b.Author == book.Author && b.Id != id))
            return Conflict("A book with this title and author already exists.");

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Book>> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return Ok(book);
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

    // PATCH: api/books/{id}/sold
    [HttpPatch("{id}/sold")]

    public async Task<IActionResult> MarkAsSold(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();

        book.IsSold = true;
        await _context.SaveChangesAsync();
        return NoContent();

    }

    [HttpPatch("{id}/available")]
    public async Task<IActionResult> MarkAsAvailable(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();

        book.IsSold = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}