using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SignalRWebUI.Dtos.CategoryDtos;
using SignalRWebUI.Dtos.ProductDtos;
using System.Text;
using System.Collections.Generic; // List için eklendi
using System.Linq; // LINQ için eklendi
using System.Threading.Tasks; // async/await için eklendi

namespace SignalRWebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Ürünleri kategori adlarıyla birlikte listeleme
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7000/api/Products/ProductsListWithCategory");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                // API'den gelen verinin hangi DTO'ya mapleneceği kontrol edilmeli, 
                // ProductsListWithCategory için ResultProductWithCategoryDto kullanılıyor olmalı.
                // Not: Koddaki DTO'yu ResultProductDto olarak tuttum.
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        // Yeni ürün oluşturma sayfasını yükleme (GET)
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7000/api/Categories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);

                // Kategori listesini SelectListItem'e dönüştürüp ViewBag'e atama
                List<SelectListItem> categoryList = (from x in values
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryId.ToString()
                                                     }).ToList();
                ViewBag.v = categoryList;
            }
            // Başarısız olursa boş bir liste göndermek yerine, loglama yapılabilir.
            return View();
        }

        // Yeni ürün oluşturma işlemini kaydetme (POST)
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            createProductDto.ProductStatus = true; // Yeni ürünler için varsayılan durum
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createProductDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Post işlemini await ile bekleyerek potansiyel deadlock sorununu çözdük.
            var responseMessage = await client.PostAsync("https://localhost:7000/api/Products/", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // Ürünü silme
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7000/api/Products/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // Ürün güncelleme sayfasını yükleme (GET)
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // Kategori listesini almak ve ViewBag'e atamak
            var categoriesResponse = await client.GetAsync("https://localhost:7000/api/Categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesJsonData = await categoriesResponse.Content.ReadAsStringAsync();
                var categoryValues = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoriesJsonData);
                List<SelectListItem> categoryList = (from x in categoryValues
                                                     select new SelectListItem
                                                     {
                                                         Text = x.CategoryName,
                                                         Value = x.CategoryId.ToString()
                                                     }).ToList();
                ViewBag.v = categoryList;
            }

            // Güncellenecek ürün verisini API'den çekme
            var productResponse = await client.GetAsync($"https://localhost:7000/api/Products/{id}");
            if (productResponse.IsSuccessStatusCode)
            {
                var productJsonData = await productResponse.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<UpdateProductDto>(productJsonData);
                return View(values);
            }
            return View();
        }

        // Ürün güncelleme işlemini kaydetme (POST)
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateProductDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7000/api/Products", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
