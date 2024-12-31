using AutoMapper;
using Client.Models.BookEntities.Book;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.DTOs.Book;

namespace Client.Controllers
{
    [Authorize(Roles = "admin")]
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly IMapper _mapper;

        public BookController(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _baseAddress = "https://localhost:7007/gateway/books"; 
            _mapper = mapper;
        }


        public async Task<IActionResult> GetAllBooks(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(_baseAddress + queryString);

                if (response.IsSuccessStatusCode)
                {
                    var books = await response.Content.ReadFromJsonAsync<List<Book>>();
                    return View(books);
                }
                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while fetching books.");
            }
        }

        public async Task<IActionResult> GetBookById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Book ID cannot be empty.");

            var book = await FetchBookByIdAsync(id);

            if (book == null)
                return NotFound($"Book with ID [{id}] not found.");

            return View("Manage", book);
        }

        public IActionResult CreateBook()
        {
            ViewBag.IsPost = true;
            return View("Manage", new ManageBookViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(ManageBookViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseAddress, _mapper.Map<Book>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllBooks));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while creating the book.");
            }
        }

        public async Task<IActionResult> UpdateBook(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Book ID cannot be empty.");

            var book = await FetchBookByIdAsync(id);

            if (book == null)
                return NotFound($"Book with ID [{id}] not found.");

            ViewBag.IsPost = false;
            return View("Manage", _mapper.Map<ManageBookViewModel>(book));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(Guid id, ManageBookViewModel model)
        {
            if (!ModelState.IsValid || id != model.Id)
                return BadRequest("Invalid book data or mismatched ID.");

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseAddress}/{id}", _mapper.Map<Book>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllBooks));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while updating the book.");
            }
        }

        public async Task<IActionResult> DeleteBook(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Book ID cannot be empty.");

            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseAddress}/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllBooks));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while deleting the book.");
            }
        }


        private async Task<Book?> FetchBookByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseAddress}/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<Book>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IActionResult HandleErrorResponse(HttpResponseMessage response)
        {
            var errorMessage = response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => "Invalid request.",
                System.Net.HttpStatusCode.NotFound => "Resource not found.",
                System.Net.HttpStatusCode.InternalServerError => "Server encountered an error.",
                _ => "An unexpected error occurred."
            };

            return View("Error", new ErrorViewModel { Message = errorMessage });
        }

        private IActionResult HandleException(Exception ex, string defaultMessage)
        {
            return View("Error", new ErrorViewModel
            {
                Message = defaultMessage,
                Details = ex.Message
            });
        }
    }
}
