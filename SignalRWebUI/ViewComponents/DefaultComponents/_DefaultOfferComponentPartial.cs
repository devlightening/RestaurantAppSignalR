using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.DiscountDtos;

namespace SignalRWebUI.ViewComponents.DefaultComponents
{
    public class _DefaultOfferComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public _DefaultOfferComponentPartial(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7000/Api/Discounts");

            if (!responseMessage.IsSuccessStatusCode)
                return View(new List<ResultDiscountDto>());

            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultDiscountDto>>(jsonData)
                          ?.Where(x => x.Status)   // sadece aktif olanları al
                          .Take(2)                 // en fazla 2 tanesini göster
                          .ToList() ?? new();

            return View(values);
        }
    }
}
