using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SimpleMvcApp.Data;
using SimpleMvcApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SimpleMvcApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public ProductController(IHttpClientFactory httpClientFactory , AppDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7125");
            _context = context;
        }
        [AllowAnonymous]
        public async Task<IActionResult> ApiIndex()
        {
           var products = await _httpClient.GetFromJsonAsync<List<Product>>("api/product");
            return View(products);
        }

        public async Task<IActionResult> Index(int page=1 , int pageSize=5)
        {
            var username = User.Identity?.Name ?? HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            int totalitems = await _context.Products.CountAsync();

            var products = await _context.Products
                .OrderBy(p => p.id)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalitems / pageSize);


            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")) && !string.IsNullOrEmpty(username))
            //{
            //    HttpContext.Session.SetString("Username", username);
            //}

            //var products = await _context.Products.ToListAsync();
            //ViewBag.Username = username;

            return View(products);

        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Deletecnf(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));

        }
        

        //private static List<Product> productlist = new List<Product>
        //{
        //    new Product { id = 1, Name = "Loptop", Price = 999.99m, Category = "Electronics", IsInStock = true },
        //    new Product { id = 2, Name = "Wireless Mouse", Price = 90.99m, Category = "Accessories", IsInStock = true },
        //    new Product { id = 3, Name = "Earphones", Price = 95.99m, Category = "Electronics", IsInStock = true },
        //    new Product { id = 4, Name = "Loptop", Price = 99.99m, Category = "Electronics", IsInStock = false }
        //};

        //public IActionResult Index()
        //{
        //    return View(productlist);
        //}

        //// GET: Product/Create
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Product/Create
        //[HttpPost]
        //public IActionResult Create(Product product)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(product);
        //    }

        //    if (productlist.Count > 0)
        //    {
        //        product.id = productlist.Max(p => p.id) + 1;
        //    }
        //    else
        //    {
        //        product.id = 1;
        //    }

        //    productlist.Add(product);

        //    return RedirectToAction("Index");
        //}

        //// GET: Product/Delete/3
        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    var product = productlist.FirstOrDefault(p => p.id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Product/Delete/3
        //[HttpPost, ActionName("Delete")]
        //public IActionResult Deletecnf(int id)
        //{
        //    var product = productlist.FirstOrDefault(p => p.id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    productlist.Remove(product);

        //    return RedirectToAction("Index");
        //}
        //public IActionResult Details(int id)
        //{
        //    var product = productlist.FirstOrDefault(p => p.id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var product = productlist.FirstOrDefault(p => p.id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product product)
        //{
        //    var existingproduct = productlist.FirstOrDefault(p => p.id == product.id);
        //    Console.WriteLine("Existing Product:");
        //    Console.WriteLine($"Id: {existingproduct?.id}");
        //    Console.WriteLine($"Name: {existingproduct?.Name}");
        //    Console.WriteLine($"Price: {existingproduct?.Price}");
        //    Console.WriteLine($"Category: {existingproduct?.Category}");
        //    Console.WriteLine($"IsInStock: {existingproduct?.IsInStock}");
        //    //Console.WriteLine($"IsInStock: {product?.Name}");

        //    if (existingproduct == null)
        //    {
        //        return NotFound();
        //    }
        //    existingproduct.Name = product.Name;
        //    existingproduct.Price = product.Price;
        //    existingproduct.Category = product.Category;
        //    existingproduct.IsInStock = product.IsInStock;

        //    return RedirectToAction("Index");
        //}
    }
    }

