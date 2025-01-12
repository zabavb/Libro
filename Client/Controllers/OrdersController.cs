using AutoMapper;
using Client.Extensions;
using Client.Models;
using Client.Models.OrderEntities.Order;
using Library.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    // Ideas:
    // make use of [AllowAnonymous] to allows users fetch their orders
    // make use of policies in the future 
    // find a way to make a fetch request with current user id as a filter
    [Authorize(Roles = "admin")] // Under question
    public class OrdersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly IMapper _mapper;
        public OrdersController(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _baseAddress = "https://localhost:7007/gateway/orders";
            _mapper = mapper;
        }

        //=========================== Actions ===========================

        public async Task<IActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? filter = null)
        {
            try
            {
                var queryString = BuildQueryString(pageNumber, pageSize, searchTerm, filter);
                var response = await _httpClient.GetAsync(_baseAddress + queryString);

                if (response.IsSuccessStatusCode)
                {
                    var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
                    return View(orders);
                }
                return ErrorHandlers.HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return ErrorHandlers.HandleException(ex, "An error occurred while fetching orders.");
            }
        }

        public async Task<IActionResult> GetOrderById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Order ID cannot be empty.");

            var order = await FetchOrderByIdAsync(id);

            if (order == null)
                return NotFound($"Order with ID [{id}] not found.");
            
            return View("Manage", order);
        }

        public IActionResult CreateOrder()
        {
            ViewBag.IsPost = true;
            return View("Manage", new ManageOrderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(ManageOrderViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseAddress, _mapper.Map<Order>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllOrders));

                return ErrorHandlers.HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return ErrorHandlers.HandleException(ex, "An error occurred while creating the order.");
            }
        }

        public async Task<IActionResult> UpdateOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Order ID cannot be empty.");

            var order = await FetchOrderByIdAsync(id);

            if (order == null)
                return NotFound($"Order with ID [{id}] not found.");

            ViewBag.IsPost = false;
            return View("Manage", _mapper.Map<ManageOrderViewModel>(order));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Guid id, ManageOrderViewModel model)
        {
            if (!ModelState.IsValid || id != model.Id)
                return BadRequest("Invalid order data or mismatched ID.");

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseAddress}", _mapper.Map<Order>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllOrders));

                return ErrorHandlers.HandleErrorResponse(response);
            }
            catch(Exception ex)
            {
                return ErrorHandlers.HandleException(ex, "An error occured while updating the order.");
            }
        }

        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Order ID cannot be empty.");

            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseAddress}/{id}");

                if(response.IsSuccessStatusCode)
                    return RedirectToAction($"{nameof(GetAllOrders)}");

                return ErrorHandlers.HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return ErrorHandlers.HandleException(ex, "An error occured while deleting the order.");
            }
        }

        // =========================== Functions ===========================
        private async Task<Order?> FetchOrderByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            try
            {
                var response = await _httpClient.GetAsync($"{_baseAddress}/${id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<Order>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string BuildQueryString(int pageNumber, int pageSize, string? searchTerm, string? filter)
        {
            var queryParams = new List<string> { $"pageNumber={pageNumber}", $"pageSize={pageSize}" };
        
            if(!string.IsNullOrEmpty(searchTerm))
                queryParams.Add($"searchTerm={searchTerm}");
            if (!string.IsNullOrEmpty(filter))
                queryParams.Add($"filter={filter}");

            return "?" + string.Join("&", queryParams);
        }

    }

}
