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
            // Model geçerli değilse ürün listesini tekrar yükle ve view'a geri dön
            if (!ModelState.IsValid)
            {
                model.Products = await LoadProductsAsync();
                return View(model);
            }

            // Seçilen ürün Id'lerini Order DTO'suna ekle
            model.Order.ProductIds = model.SelectedProductIds;

            // Seçilen ürünleri API'den topluca çekme imkânınız yoksa şu anki yöntem çalışır,
            // ama daha performanslı olması için tek seferde çekme API'si tercih edilmeli
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
                    else
                    {
                        ModelState.AddModelError("", $"Ürün bilgisi alınamadı: {productId}");
                    }
                }

                // Toplam fiyatı hesapla
                model.Order.TotalOrderPrice = selectedProducts.Sum(p => p.Price);
            }
            else
            {
                ModelState.AddModelError("", "En az bir ürün seçmelisiniz.");
                model.Products = await LoadProductsAsync();
                return View(model);
            }

            // Tarih formatı kontrolü (opsiyonel, backend'de parse sorun varsa buraya eklenebilir)

            // Siparişi API'ye gönder
            var postClient = _httpClientFactory.CreateClient();
            var jsonOrder = JsonConvert.SerializeObject(model.Order);
            var stringContent = new StringContent(jsonOrder, System.Text.Encoding.UTF8, "application/json");

            var responseMessage = await postClient.PostAsync("https://localhost:7000/api/Orders", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Başarısızlık durumunda response içeriğini logla veya modele ekle (debug için)
                var errorContent = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Sipariş kaydedilemedi: {errorContent}");
            }

            // Ürünleri yeniden yükle ve view'a dön
            model.Products = await LoadProductsAsync();
            return View(model);
        }

        // Yardımcı method: ürün listesini yükle
        private async Task<List<ResultProductDto>> LoadProductsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7000/api/Products");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
            }
            return new List<ResultProductDto>();
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

