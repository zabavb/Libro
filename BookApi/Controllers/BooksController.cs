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


        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Retrieves a paginated list of books based on the specified filters, search term, and sorting options.
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1). The page number to retrieve.</param>
        /// <param name="pageSize">Number of books per page (default: 10). The number of books to return per page.</param>
        /// <param name="searchTerm">Search term (optional). A string to search in the book's title, author, or other properties.</param>
        /// <param name="filter">Filter options (optional). Filters to apply on the books, such as author, price range, etc.</param>
        /// <param name="sort">Sort options (optional). Specifies the sorting order, such as by title or price.</param>
        /// <returns>A paginated list of books.</returns>
        /// <response code="200">Returns a list of books according to the specified pagination, filter, and sort parameters.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] Filter? filter = null, [FromQuery] Sort? sort = null)
        {
            var books = await _bookService.GetBooksAsync(pageNumber, pageSize, searchTerm, filter, sort);
            return Ok(books);
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
