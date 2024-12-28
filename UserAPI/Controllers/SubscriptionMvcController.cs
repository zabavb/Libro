using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    public class SubscriptionMvcController : Controller
    {
        private readonly HttpClient _httpClient;

        public SubscriptionMvcController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Subscription");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<SubscriptionViewModel>());
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var subscriptions = JsonConvert.DeserializeObject<List<SubscriptionViewModel>>(jsonData);
            return View(subscriptions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubscriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _httpClient.PostAsJsonAsync("api/Subscription", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error creating subscription.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Subscription/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var subscription = JsonConvert.DeserializeObject<SubscriptionViewModel>(jsonData);
            return View(subscription);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, SubscriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _httpClient.PutAsJsonAsync($"api/Subscription/{id}", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error updating subscription.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Subscription/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error deleting subscription.");
            }

            return RedirectToAction("Index");
        }
    }
}
