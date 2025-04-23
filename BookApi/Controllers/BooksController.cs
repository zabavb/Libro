using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.DTOs.Book;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace BookAPI.Controllers
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
        private readonly IDiscountService _discountService;
        private readonly ILogger<BooksController> _logger;


        public BooksController(IBookService bookService, ILogger<BooksController> logger,
            IDiscountService discountService)
        {
            _bookService = bookService;
            _logger = logger;
            _discountService = discountService;
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
            [FromQuery] BookFilter? filter = null,
            [FromQuery] BookSort? sort = null
        )
        {
            try
            {
                var books = await _bookService.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);
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
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            try
            {
                var book = await _bookService.GetByIdAsync(id);

                if (book == null)
                {
                    return NotFound($"Book with id {id} not found.");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves Book names for user's details page by ID.
        /// </summary>
        /// <param name="ids">The unique identifiers of books which titles to retrieve.</param>
        /// <returns>Books' titles which IDs matches with provided ones in parameters.</returns>
        /// <response code="200">Retrieval successful, return the book titles.</response>
        /// <response code="500">An unexpected error occured.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet("for-user/details")]
        public async Task<ActionResult<ICollection<string>>> GetAllForUserDetailsAsync(
            [FromQuery] ICollection<Guid> ids)
        {
            var titles = await _bookService.GetAllForUserDetailsAsync(ids);
            return Ok(titles);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="bookDto">Book data.</param>
        /// <returns>The created book.</returns>
        /// <response code="201">Book successfully created.</response>
        /// <response code="400">Invalid input data.</response>
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromForm] BookDto bookDto, IFormFile? imageFile)
        {
            if (bookDto == null)
            {
                _logger.LogWarning("Invalid book data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                /*var createdBook = */
                await _bookService.CreateAsync(bookDto, imageFile);
                // var createdDiscount = await _discountService.AddAsync(new DiscountDTO { BookId = createdBook.BookId, DiscountRate = 0 });
                return CreatedAtAction(nameof(GetById), new { id = bookDto.BookId }, bookDto);
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
        public async Task<ActionResult<BookDto>> Update(Guid id, [FromBody] UpdateBookRequest request,
            IFormFile? imageFile)
        {
            var bookDto = request.Book;
            var discount = request.Discount;
            if (bookDto == null)
            {
                _logger.LogWarning("Invalid book data provided for update.");
                return BadRequest("Invalid data.");
            }

            try
            {
                /*var updatedBook = */
                await _bookService.UpdateAsync(id, bookDto, imageFile);

                /*if (updatedBook == null)
                {
                    return NotFound($"Book with id {id} not found.");
                }*/

                //discount.BookId = updatedBook.BookId;

                if (discount != null)
                {
                    /*_ =*/
                    await _discountService.UpdateAsync(discount);
                }

                return Ok( /*updatedBook*/);
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
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                /*var isDeleted = */
                await _bookService.DeleteAsync(id);

                /*if (!isDeleted)
                {
                    return NotFound($"Book with id {id} not found.");
                }*/

                var discount = await _discountService.GetByBookIdAsync(id);
                if (discount == null)
                {
                    return NotFound($"Discount with id {id} not found.");
                }

                /*var isDeletedDiscount = */
                await _discountService.DeleteAsync(id);

                /*if (!isDeletedDiscount)
                {
                    return NotFound($"Discount with id {discount.BookId} was not deleted.");
                }*/

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Adds quantity to a book by its ID.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <param name="quantity">Quantity to add.</param>
        /// <returns>Ok result if successful.</returns>
        /// <response code="200">Quantity successfully added.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("AddQuantity")]
        public async Task<IActionResult> AddQuantityById(Guid id, int quantity)
        {
            try
            {
                await _bookService.AddQuantityById(id, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding quantity to book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves the quantity of a book by its ID.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <returns>The quantity of the book.</returns>
        /// <response code="200">Returns the quantity of the book.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("GetQuantityById")]
        public async Task<IActionResult> GetQuantityById(Guid id)
        {
            try
            {
                var quantity = await _bookService.GetQuantityById(id);
                return Ok(quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving quantity for book with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a list of books based on a specified condition.
        /// </summary>
        /// <param name="condition">The condition to filter books.  b => b.Quantity > 0</param>
        /// <returns>A list of books that match the specified condition.</returns>
        /// <response code="200">Returns the list of books matching the condition.</response>
        /// <response code="500">Internal server error.</response>
        public async Task<List<BookDto>> GetBooksByConditionAsync(Expression<Func<Models.Book, bool>> condition)
        {
            try
            {
                var books = await _bookService.GetBooksByConditionAsync(condition);
                return books;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving books by condition.");
                throw;
            }
        }
    }
}