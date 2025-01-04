using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;


namespace BookApi.Services
{
    public class BookService : IBookService
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public BookService(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var books = await _context.Books
                                       .Include(b => b.Category)
                                       .Include(b => b.Publisher)
                                       .Include(b => b.Feedbacks)
                                       .ToListAsync();

            if (books == null || books.Count == 0)
            {
                return [];
            }

            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync(string? searchQuery = null, string? sortBy = null)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Feedbacks)
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(searchQuery) ||
                    b.Description.ToLower().Contains(searchQuery) ||
                    b.Author.Name.ToLower().Contains(searchQuery));
            }

            query = sortBy switch
            {
                "popularity" => query.OrderByDescending(b => b.Feedbacks.Count),
                "newest" => query.OrderByDescending(b => b.Year),
                "cheapest" => query.OrderBy(b => b.Price),
                "expensive" => query.OrderByDescending(b => b.Price),
                _ => query.OrderBy(b => b.Title)
            };

            var books = await query.ToListAsync();

            return _mapper.Map<List<BookDto>>(books);
        }


        public async Task<BookDto> GetBookByIdAsync(Guid id) 
        {
            var book = await _context.Books
                                      .Include(b => b.Category)
                                      .Include(b => b.Publisher)
                                      .Include(b => b.Feedbacks)
                                      .FirstOrDefaultAsync(b => b.Id == id); 

            if (book == null)
            {
                return null; 
            }

            return _mapper.Map<BookDto>(book); 
        }
        public async Task<BookDto> CreateBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> UpdateBookAsync(Guid id, BookDto bookDto)
        {
            var existingBook = await _context.Books
                                              .Include(b => b.Category)
                                              .Include(b => b.Publisher)
                                              .Include(b => b.Feedbacks)
                                              .FirstOrDefaultAsync(b => b.Id == id);

            if (existingBook == null)
            {
                return null; 
            }

            existingBook.Title = bookDto.Title;
            existingBook.Price = bookDto.Price;
            existingBook.Language = (Language)bookDto.Language;
            existingBook.Year = bookDto.Year;
            existingBook.Description = bookDto.Description;
            existingBook.Cover = (CoverType)bookDto.Cover;
            existingBook.IsAvaliable = bookDto.IsAvaliable;
            existingBook.FeedbackIds = bookDto.FeedbackIds ?? existingBook.FeedbackIds;
            existingBook.PublisherId = bookDto.PublisherId;
            existingBook.CategoryId = bookDto.CategoryId;

            await _context.SaveChangesAsync();

            return _mapper.Map<BookDto>(existingBook);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books
                                      .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return false; 
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true; 
        }



    }

}
