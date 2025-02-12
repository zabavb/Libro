using BookApi.Models;
using BookAPI;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Services.Interfaces;
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
        private readonly ILogger<BooksController> _logger;


        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
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
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] string? searchTerm = null, 
            [FromQuery] BookFilter? filter = null
            ,
            [FromQuery] BookSort? sort = null
            )
        {
            try
            {
                var books = await _bookService.GetBooksAsync(pageNumber, pageSize, searchTerm, filter, sort);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving books.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
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
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                {
                    _logger.LogWarning($"Book with id {id} not found.");
                    return NotFound($"Book with id {id} not found.");
                }

                _logger.LogInformation($"Book with id {id} successfully fetched.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
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
                _logger.LogWarning("Invalid book data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var createdBook = await _bookService.CreateBookAsync(bookDto);
                _logger.LogInformation($"Book with id {createdBook.BookId} successfully created.");
                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.BookId }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new book.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
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
                _logger.LogWarning("Invalid book data provided for update.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);

                if (updatedBook == null)
                {
                    _logger.LogWarning($"Book with id {id} not found for update.");
                    return NotFound($"Book with id {id} not found.");
                }

                _logger.LogInformation($"Book with id {id} successfully updated.");
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
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
            try
            {
                var isDeleted = await _bookService.DeleteBookAsync(id);

                if (!isDeleted)
                {
                    _logger.LogWarning($"Book with id {id} not found for deletion.");
                    return NotFound($"Book with id {id} not found.");
                }

                _logger.LogInformation($"Book with id {id} successfully deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
