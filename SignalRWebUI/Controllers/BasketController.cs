using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.BasketDtos;

namespace SignalRWebUI.Controllers
{
    public class BasketController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BasketController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://localhost:7000/api/Baskets/BasketListByMenuTableWithProductName?id=24");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Hatası: {response.StatusCode}");
                return View(new List<ResultBasketDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var baskets = JsonConvert.DeserializeObject<List<ResultBasketDto>>(json);

            return View(baskets);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7000/api/Baskets/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return NotFound("Böyle bir ürün yok");
        }



    }
}
