    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using SignalRWebUI.Dtos.AppUserDtos;
    using System.Text;

    namespace SignalRWebUI.Controllers
    {
        public class AppUserController : Controller
        {
            private readonly IHttpClientFactory _httpClientFactory;

            public AppUserController(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }

            // Kullanıcı listesi
            public async Task<IActionResult> Index()
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7000/api/AppUsers");

                if (!response.IsSuccessStatusCode)
                    return View();

                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<ResultAppUserDto>>(json);
                return View(users);
            }

            // Kullanıcı oluşturma - GET
            [HttpGet]
            public IActionResult CreateAppUser()
            {
                return View();
            }

            // Kullanıcı oluşturma - POST
            [HttpPost]
            public async Task<IActionResult> CreateAppUser(CreateAppUserDto createAppUserDto)
            {
                var client = _httpClientFactory.CreateClient();
                var json = JsonConvert.SerializeObject(createAppUserDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7000/api/AppUsers", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Kullanıcı oluşturulamadı. Lütfen bilgileri kontrol edin.");
                return View(createAppUserDto);
            }

            // Kullanıcı silme - POST
            [HttpPost]
            public async Task<IActionResult> DeleteAppUser(int id)
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7000/api/AppUsers/{id}");

                if (!response.IsSuccessStatusCode)
                    ModelState.AddModelError("", "Kullanıcı silinirken hata oluştu.");

                return RedirectToAction(nameof(Index));
            }

            // Kullanıcı güncelleme - GET
            [HttpGet]
            public async Task<IActionResult> UpdateAppUser(int id)
            {
                var client = _httpClientFactory.CreateClient();
                var responseMessage = await client.GetAsync($"https://localhost:7000/api/AppUsers/{id}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<UpdateAppUserDto>(jsonData);
                    return View(values);
                }
                return View();
            }

        // Kullanıcı güncelleme - POST
        [HttpPost]
        public async Task<IActionResult> UpdateAppUser(UpdateAppUserDto updateDto)
        {
            if (!ModelState.IsValid)
                return View(updateDto); // Hatalıysa form geri gönderilir

            try
            {
                var client = _httpClientFactory.CreateClient();

                // JSON olarak DTO'yu serialize et
                var json = JsonConvert.SerializeObject(updateDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // PUT isteği gönder
                var response = await client.PutAsync($"https://localhost:7000/api/AppUsers/{updateDto.AppUserId}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }

                // Sunucu tarafı hata mesajını alabiliriz
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Sunucu hatası: {errorMessage}");
            }
            catch (Exception ex)
            {
                // Ağ veya başka bir hata oluşursa
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
            }

            return View(updateDto);
        }

    }
}
