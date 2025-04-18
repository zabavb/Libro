using AutoMapper;
using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;
using Library.Common;
using System.Linq.Expressions;

namespace BookAPI.Services
{
    public class BookService(
        IBookRepository bookRepository,
        IMapper mapper,
        ILogger<BookService> logger,
        S3StorageService storageService) : IBookService
    {
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BookService> _logger = logger;
        private readonly S3StorageService _storageService = storageService;

        public async Task<PaginatedResult<BookDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            BookFilter? filter,
            BookSort? sort)
        {
            var books = await _bookRepository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

            if (books == null || books.Items == null)
            {
                _logger.LogWarning("No books found");
                throw new InvalidOperationException("Failed to fetch books.");
            }

            _logger.LogInformation("Successfully found books");
            return new PaginatedResult<BookDto>
            {
                Items = _mapper.Map<ICollection<BookDto>>(books.Items),
                TotalCount = books.TotalCount,
                PageNumber = books.PageNumber,
                PageSize = books.PageSize
            };
        }

        public async Task<BookDto> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning($"No book with id {id}");
                return null;
            }

            _logger.LogInformation($"Successfully found book with id {id}");
            return _mapper.Map<BookDto>(book);
        }

        public async Task<ICollection<string>?> GetAllForUserDetailsAsync(ICollection<Guid> ids)
        {
            try
            {
                return await _bookRepository.GetAllForUserDetailsAsync(ids);
            }
            catch
            {
                return null;
            }
        }

        public async Task /*<BookDto>*/ CreateAsync(BookDto bookDto, IFormFile? imageFile)
        {
            var book = _mapper.Map<Book>(bookDto);
            book.Id = Guid.NewGuid();
            if (imageFile != null)
            {
                book.ImageUrl = await UploadImageAsync(imageFile, book.Id);
            }

            try
            {
                await _bookRepository.CreateAsync(book);
                _logger.LogInformation($"Successfully created book with id {book.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create book. Error: {ex.Message}");
                throw;
            }

            // return _mapper.Map<BookDto>(book);
        }

        public async Task /*<BookDto>*/ UpdateAsync(Guid id, BookDto bookDto, IFormFile? imageFile)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);

            if (existingBook == null)
            {
                _logger.LogWarning($"Book with id {id} not found.");
                throw new InvalidOperationException($"Book with id {id} not found.");
            }

            try
            {
                if (!string.IsNullOrEmpty(existingBook.ImageUrl))
                {
                    await _storageService.DeleteAsync(GlobalConstants.bucketName, existingBook.ImageUrl);
                }

                if (imageFile != null)
                {
                    existingBook.ImageUrl = await UploadImageAsync(imageFile, id);
                }

                _mapper.Map(bookDto, existingBook);
                await _bookRepository.UpdateAsync(existingBook);
                _logger.LogInformation($"Successfully updated book with id {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update book. Error: {ex.Message}");
                throw;
            }

            // return _mapper.Map<BookDto>(existingBook);
        }

        public async Task /*<bool>*/ DeleteAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning($"Book with id {id} not found.");
                // return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    await _storageService.DeleteAsync(GlobalConstants.bucketName, book.ImageUrl);
                }

                await _bookRepository.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted book with id {id}");
                // return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete book. Error: {ex.Message}");
                // return false;
            }
        }

        private async Task<string?> UploadImageAsync(IFormFile? imageFile, Guid id)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            try
            {
                return await _storageService.UploadAsync(GlobalConstants.bucketName, imageFile, "book/images/", id);
            }
            catch (Exception ex)
            {
                string message = "Error occurred while uploading book's image.";
                _logger.LogError(message);
                throw new InvalidOperationException(message, ex);
            }
        }
        // example of condition: b => b.Quantity > 0
        public async Task<List<BookDto>> GetBooksByConditionAsync(Expression<Func<Book, bool>> condition)
        {

            var books = await _bookRepository.GetBooksByConditionAsync(condition);

            if (books == null || books.Count == 0)
            {
                _logger.LogWarning("No books matched the given condition");
                throw new InvalidOperationException("No matching books found.");
            }

            _logger.LogInformation("Found {Count} books", books.Count);

            return _mapper.Map<List<BookDto>>(books);
        }


        public async Task<int> GetQuantityById(Guid id)
        {
            var quantity = await _bookRepository.GetQuantityById(id);
            if (quantity == 0)
            {
                _logger.LogWarning($"No book with id {id}");
                throw new InvalidOperationException($"No book with id {id}");
            }
            _logger.LogInformation($"Successfully found quantity of book with id {id}");
            return quantity;
        }

        public async Task AddQuantityById(Guid id, int quantity)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"No book with id {id}");
                throw new InvalidOperationException($"No book with id {id}");
            }
            book.Quantity += quantity;
            await _bookRepository.UpdateAsync(book);
            _logger.LogInformation($"Successfully added {quantity} to book with id {id}");
        }
    }
}
