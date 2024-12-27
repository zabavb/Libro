using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    /// <summary>
    /// Manages book-related operations such as retrieving, creating, updating, and deleting books.
    /// </summary>
    /// <remarks>
    /// This controller provides CRUD operations for managing books in the system.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;


        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Retrieves a paginated list of books.
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1).</param>
        /// <param name="pageSize">Number of books per page (default: 10).</param>
        /// <param name="searchQuery"></param>
        /// <param name="sortBy"></param>
        /// <returns>A paginated list of books.</returns>
        /// <response code="200">Returns the list of books.</response>
        /// <response code="400">Invalid pagination parameters.</response>
        /// <response code="404">No books found.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize, string? searchQuery = null, string? sortBy = null)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var books = await _bookService.GetBooksAsync(searchQuery, sortBy);

            if (books == null || !books.Any())
            {
                return NotFound("No books found.");
            }

            var paginatedBooks = books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(paginatedBooks);
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <returns>The requested book.</returns>
        /// <response code="200">Returns the book.</response>
        /// <response code="404">Book not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound($"Book with id {id} not found.");
            }

            return Ok(book);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="bookDto">Book data.</param>
        /// <returns>The created book.</returns>
        /// <response code="201">Book successfully created.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var createdBook = await _bookService.CreateBookAsync(bookDto);

            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.BookId }, createdBook);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <param name="bookDto">Updated book data.</param>
        /// <returns>The updated book.</returns>
        /// <response code="200">Book successfully updated.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Book not found.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> UpdateBook(Guid id, [FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);

            if (updatedBook == null)
            {
                return NotFound("Book not found.");
            }

            return Ok(updatedBook);
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Book successfully deleted.</response>
        /// <response code="404">Book not found.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            var isDeleted = await _bookService.DeleteBookAsync(id);

            if (!isDeleted)
            {
                return NotFound("Book not found.");
            }

            return NoContent();
        }
    }
}
