using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.UI.ViewModels;

namespace ShopApp.UI.Controllers
{
    public class BrandController : Controller
    {
        private HttpClient _clinet;
        public BrandController()
        {
            _clinet = new HttpClient();
            _clinet.BaseAddress = new Uri("https://localhost:7172/api/");
        }
        public async Task<IActionResult> Index(List<int> tagIds,string search)
        {
            ViewBag.TagIds = tagIds;
            ViewBag.Search = search;
            using(HttpClient client = new HttpClient())
            {
                using(var response = await client.GetAsync("https://localhost:7172/api/brands/all"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        BrandViewModel vm = new BrandViewModel
                        {
                            Brands = JsonConvert.DeserializeObject<List<BrandViewModelItem>>(content)
                        };

                        return View(vm);
                    }
                }
            }

            return View("error");
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View();
            using (HttpClient client = new HttpClient())
            {
                StringContent requestContent = new StringContent(JsonConvert.SerializeObject(vm), System.Text.Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7172/api/brands", requestContent))
                {
                    if(response.IsSuccessStatusCode)
                        return RedirectToAction("index");
                    else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var errorVM = JsonConvert.DeserializeObject<ErrorViewModel>(responseContent);

                        foreach (var item in errorVM.Errors)
                            ModelState.AddModelError(item.Key, item.ErrorMessage);

                        return View();
                    }
                }
            }
            return View("Error");
        }

        public async Task<IActionResult> Edit(int id)
        {
            using(var response = await _clinet.GetAsync($"brands/{id}"))
            {
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(); 
                    var vm = JsonConvert.DeserializeObject<BrandEditViewModel>(content);
                    return View(vm);
                }
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BrandEditViewModel vm)
        {
            if (!ModelState.IsValid) return View();

            var requestContent = new StringContent(JsonConvert.SerializeObject(vm), System.Text.Encoding.UTF8, "application/json");
            using(var response = await _clinet.PutAsync($"brands/{id}", requestContent))
            {
                if(response.IsSuccessStatusCode)
                    return RedirectToAction("index");
                else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var errorVM = JsonConvert.DeserializeObject<ErrorViewModel>(responseContent);

                    foreach (var item in errorVM.Errors)
                        ModelState.AddModelError(item.Key, item.ErrorMessage);

                    return View();
                }
            }

            return View("error");
        }
    }
}
