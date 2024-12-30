using AutoMapper;
using Client.Models;
using Client.Models.OrderEntities.DeliveryType;
using Library.OrderEntities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Client.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly IMapper _mapper;
        public DeliveryController(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _baseAddress = "https://localhost:7007/gateway/DeliveryTypes";
            _mapper = mapper;
        }
        //=========================== Actions ===========================

        public async Task<IActionResult> GetAllDeliveryTypes(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var queryString = BuildQueryString(pageNumber, pageSize);
                var response = await _httpClient.GetAsync(_baseAddress + queryString);

                if (response.IsSuccessStatusCode)
                {
                    var deliveryTypes = await response.Content.ReadFromJsonAsync<List<DeliveryType>>();
                    return View(deliveryTypes);
                }
                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while fetching delivery types.");
            }
        }

        public async Task<IActionResult> GetDeliveryTypeById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Delivery type ID cannot be empty.");

            var deliveryType = await FetchDeliveryTypeByIdAsync(id);

            if (deliveryType == null)
                return NotFound($"Delivery type with ID [{id}] not found.");

            return View("Manage", deliveryType);
        }

        public IActionResult CreateDeliveryType()
        {
            ViewBag.IsPost = true;
            return View("Manage", new ManageDeliveryTypeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeliveryType(ManageDeliveryTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseAddress, _mapper.Map<DeliveryType>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllDeliveryTypes));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occurred while creating the delivery type.");
            }
        }

        public async Task<IActionResult> UpdateDeliveryType(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Delivery type ID cannot be empty.");

            var deliveryType = await FetchDeliveryTypeByIdAsync(id);

            if (deliveryType == null)
                return NotFound($"DeliveryType with ID [{id}] not found.");

            ViewBag.IsPost = false;
            return View("Manage", _mapper.Map<ManageDeliveryTypeViewModel>(deliveryType));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryType(Guid id, ManageDeliveryTypeViewModel model)
        {
            if (!ModelState.IsValid || id != model.Id)
                return BadRequest("Invalid delivery type data or mismatched ID.");

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseAddress}", _mapper.Map<DeliveryType>(model));

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(GetAllDeliveryTypes));

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occured while updating the delivery type.");
            }
        }

        public async Task<IActionResult> DeleteDeliveryType(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Delivery type ID cannot be empty.");

            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseAddress}/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction($"{nameof(GetAllDeliveryTypes)}");

                return HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                return HandleException(ex, "An error occured while deleting the delivery type.");
            }
        }

        // =========================== Functions ===========================
        private async Task<DeliveryType?> FetchDeliveryTypeByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            try
            {
                var response = await _httpClient.GetAsync($"{_baseAddress}/${id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<DeliveryType>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        // In case those function are the same just move them into shared file?
        private string BuildQueryString(int pageNumber, int pageSize)
        {
            var queryParams = new List<string> { $"pageNumber={pageNumber}", $"pageSize={pageSize}" };

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
