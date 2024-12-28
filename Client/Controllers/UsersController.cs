using AutoMapper;
using Client.Models;
using Client.Models.User;
using Library.UserEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace Client.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly IMapper _mapper;

        public UsersController(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseAddress = "https://localhost:7007/gateway/users";
            _mapper = mapper;
        }

        //=========================== Actions ===========================

        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? filter = null)
        {
            try
            {
                var queryString = BuildQueryString(pageNumber, pageSize, searchTerm, filter);
                var response = await _httpClient.GetAsync(_baseAddress + queryString);

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<User>>();
                    return View(users);
                }
                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while fetching users.");
            }
        }

        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("User ID cannot be empty.");

            var user = await FetchUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with ID [{id}] not found.");

            return View("Manage", user);
        }

        public IActionResult CreateUser()
        {
            ViewBag.IsPost = true;
            return View("Manage", new ManageUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(ManageUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseAddress, _mapper.Map<User>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllUsers));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while creating the user.");
            }
        }

        public async Task<IActionResult> UpdateUser(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("User ID cannot be empty.");

            var user = await FetchUserByIdAsync(id);

            if (user == null)
                return NotFound($"User with ID [{id}] not found.");

            ViewBag.IsPost = false;
            return View("Manage", _mapper.Map<ManageUserViewModel>(user));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(Guid id, ManageUserViewModel model)
        {
            if (!ModelState.IsValid || id != model.Id)
                return BadRequest("Invalid user data or mismatched ID.");

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseAddress}/{id}", _mapper.Map<User>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllUsers));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while updating the user.");
            }
        }

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("User ID cannot be empty.");

            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseAddress}/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllUsers));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while deleting the user.");
            }
        }

        // =========================== Functions ===========================

        private async Task<User?> FetchUserByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            try
            {
                var response = await _httpClient.GetAsync($"{_baseAddress}/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<User>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string BuildQueryString(int pageNumber, int pageSize, string? searchTerm, string? filter)
        {
            var queryParams = new List<string> { $"pageNumber={pageNumber}", $"pageSize={pageSize}" };
            
            if (!string.IsNullOrEmpty(searchTerm))
                queryParams.Add($"searchTerm={searchTerm}");
            if (!string.IsNullOrEmpty(filter))
                queryParams.Add($"filter={filter}");

            return "?" + string.Join("&", queryParams);
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