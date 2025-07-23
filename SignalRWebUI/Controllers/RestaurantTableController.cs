using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SignalR.EntityLayer.Entities;
using SignalRWebUI.Dtos.RestaurantTableDtos;
using SignalRWebUI.Dtos.RestaurantTableDtos;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class RestaurantTableController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RestaurantTableController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var locations = Enum.GetNames(typeof(TableLocation)).ToList();
            ViewBag.Locations = locations;


            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7000/api/RestaurantTables");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultRestaurantTableDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateRestaurantTable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurantTable(CreateRestaurantTableDto createRestaurantTableDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createRestaurantTableDto);
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = client.PostAsync("https://localhost:7000/api/RestaurantTables/", stringContent);
            if (responseMessage.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        
        public async Task<IActionResult> DeleteRestaurantTable(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7000/api/RestaurantTables/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        public async Task<IActionResult> UpdateRestaurantTable(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7000/api/RestaurantTables/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateRestaurantTableDto>(jsonData);

                // ViewBag'ı burada doldur
                ViewBag.Locations = Enum.GetValues(typeof(TableLocation))
                    .Cast<TableLocation>()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }).ToList();

                return View(values);
            }

            return View(); // ViewBag burada da null olmamalı
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRestaurantTable(UpdateRestaurantTableDto updateRestaurantTableDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateRestaurantTableDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7000/api/RestaurantTables/", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Hata varsa ViewBag tekrar set edilmeli
            ViewBag.Locations = Enum.GetValues(typeof(TableLocation))
                .Cast<TableLocation>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                }).ToList();

            return View(updateRestaurantTableDto);
        }

    }
}
