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
        [HttpGet("discount/{bookId}")]
        public async Task<ActionResult<DiscountDTO>> GetDiscountByBookId(Guid bookId)
        {
            try
            {
                if (bookId == Guid.Empty)
                    return BadRequest("Некоректний ідентифікатор книги.");

                var discount = await _bookService.GetDiscountByBookIdAsync(bookId);

                if (discount == null)
                    return NotFound($"Знижка для книги з ID {bookId} не знайдена.");

                return Ok(discount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving book with id {bookId}.");
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
        /// <param name="request">Book data.</param>
        /// <returns>The created book.</returns>
        /// <response code="201">Book successfully created.</response>
        /// <response code="400">Invalid input data.</response>

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<BookDto>> Create([FromForm] BookRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Invalid book data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                await _bookService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = request.BookId }, request);
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
        /// <param name="request">Updated book data.</param>
        /// <returns>The updated book.</returns>
        /// <response code="200">Book successfully updated.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Book not found.</response>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(Guid id, [FromForm] UpdateBookRequest request)
        {
            var bookDto = new BookRequest()
            {
                BookId = request.BookId,
                Title = request.Title,
                AuthorId = request.AuthorId,
                PublisherId = request.PublisherId,
                CategoryId = request.CategoryId,
                DiscountId = request.DiscountId,
                Price = request.Price,
                Language = request.Language,
                Year = request.Year,
                Description = request.Description,
                Cover = request.Cover,
                Quantity = request.Quantity,
                Image = request.Image,
            };
            var discount = request.Discount;
            if (bookDto == null)
            {
                _logger.LogWarning("Invalid book data provided for update.");
                return BadRequest("Invalid data.");
            }
            try
            {
                /*TO BE UPDATED ON FRONTEND*/
                /*await _bookService.UpdateWithDiscountAsync(id, request, _discountService);
                return Ok();*/
                await _bookService.UpdateAsync(id, bookDto);

                if (discount != null)
                {
                    await _discountService.UpdateAsync(discount);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update book with discount");
                return BadRequest(ex.Message);
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
                await _bookService.DeleteAsync(id);

                var discount = await _discountService.GetByBookIdAsync(id);
                if (discount == null)
                {
                    return NotFound($"Discount with id {id} not found.");
                }

                await _discountService.DeleteAsync(id);
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