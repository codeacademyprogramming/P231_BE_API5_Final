using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.UI.ViewModels;
using System.Net.Http.Headers;

namespace ShopApp.UI.Controllers
{
    public class ProductController : Controller
    {
        private HttpClient _client;
        public ProductController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7172/api/");
        }
        public async Task<IActionResult> Index(int page=1)
        {
            using(var response = await _client.GetAsync($"products?page={page}"))
            {
                if(response.IsSuccessStatusCode)
                { 
                    var content = await response.Content.ReadAsStringAsync();
                    PaginatedViewModel<ProductViewModelItem> vm = JsonConvert.DeserializeObject<PaginatedViewModel<ProductViewModelItem>>(content);
                    return View(vm);
                }
            }

            return View("Error");
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Brands = await _getBrands();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = await _getBrands();
                return View();
            }

            MultipartFormDataContent requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(vm.Name), "Name");
            requestContent.Add(new StringContent(vm.BrandId.ToString()), "BrandId");
            requestContent.Add(new StringContent(vm.SalePrice.ToString()), "SalePrice");
            requestContent.Add(new StringContent(vm.CostPrice.ToString()), "CostPrice");
            var fileContent = new StreamContent(vm.ImageFile.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(vm.ImageFile.ContentType); ;
            requestContent.Add(fileContent, "ImageFile",vm.ImageFile.FileName);


            using (var response = await _client.PostAsync("products",requestContent))
            {
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("index");
                else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Brands = await _getBrands();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var errorVM = JsonConvert.DeserializeObject<ErrorViewModel>(responseContent);
                    foreach (var item in errorVM.Errors)
                        ModelState.AddModelError(item.Key, item.ErrorMessage);

                    return View();
                }
            }

            return View("error");
        }

        private async Task<List<BrandViewModelItem>> _getBrands()
        {
            List<BrandViewModelItem> data = new List<BrandViewModelItem>();
            using(var response = await _client.GetAsync("brands/all"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<BrandViewModelItem>>(content);
                }
            }

            return data;
        }
    }
}
