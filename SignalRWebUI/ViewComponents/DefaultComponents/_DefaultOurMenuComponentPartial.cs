using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.ProductDtos;

namespace SignalRWebUI.ViewComponents.DefaultComponents
{
    public class _DefaultOurMenuComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DefaultOurMenuComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            // Kategori bilgisi içeren endpoint
            var response = await client.GetAsync("https://localhost:7000/api/Products/ProductsListWithCategory");
            if (!response.IsSuccessStatusCode) return View(new List<ResultProductDto>());

            var json = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ResultProductDto>>(json);

            return View(products);
        }
    }
}
