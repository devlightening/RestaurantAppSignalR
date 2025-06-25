using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.OrderDtos;
using SignalRWebUI.Dtos.ProductDtos;
using SignalRWebUI.ViewModels;
using System.Text;

namespace SignalRWebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7000/api/Orders");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultOrderDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7000/api/Products");

            var model = new CreateOrderViewModel();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.Products = JsonConvert.DeserializeObject<List<ResultProductDto>>(json);
            }

            model.Order = new CreateOrderDto
            {
                OrderDate = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ürün listesi yeniden yüklenmeli çünkü postta gelmez
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7000/api/Products");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    model.Products = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                }

                return View(model);
            }

            // Ürünleri al
            var selectedProducts = new List<ResultProductDto>();
            if (model.SelectedProductIds != null && model.SelectedProductIds.Any())
            {
                var client = _httpClientFactory.CreateClient();

                foreach (var productId in model.SelectedProductIds)
                {
                    var response = await client.GetAsync($"https://localhost:7000/api/Products/{productId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var product = JsonConvert.DeserializeObject<ResultProductDto>(json);
                        if (product != null)
                            selectedProducts.Add(product);
                    }
                }

                // Toplam tutarı hesapla
                model.Order.TotalOrderPrice = selectedProducts.Sum(p => p.Price);
            }

            // Siparişi gönder
            var postClient = _httpClientFactory.CreateClient();
            var jsonOrder = JsonConvert.SerializeObject(model.Order);
            var stringContent = new StringContent(jsonOrder, System.Text.Encoding.UTF8, "application/json");

            var responseMessage = await postClient.PostAsync("https://localhost:7000/api/Orders", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("CreateOrder", model);

        }


        public async Task<IActionResult> DeleteOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7000/api/Orders/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> UpdateOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7000/api/Orders/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateOrderDto>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateOrderDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7000/api/Orders/", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        
    }
}

