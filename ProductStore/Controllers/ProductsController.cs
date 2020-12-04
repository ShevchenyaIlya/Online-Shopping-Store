using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProductStore.Data;
using ProductStore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ProductStore.Services;
using ProductStore.Repositories;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("Admin/Products")]
    [Route("Admin/Product")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ImageService _imageService;

        [TempData]
        public string StatusMessage { get; set; }

        public ProductsController(ImageService imageService, IProductRepository productRepository,
            ICategoryRepository categoryRepository, ICountryRepository countryRepository)
        {
            _productRepository = productRepository;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
            _imageService = imageService;
        }

        // GET: Products
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _productRepository.GetProducts());
        }

        // GET: Products/Details/5
        [Route("Details/{id:long:min(1)?}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByID(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(product);
        }

        // GET: Products/Create
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            foreach (var category in await _categoryRepository.GetCategories())
            {
                categories.Add(new SelectListItem() { Value = category.CategoryName, Text = category.CategoryName });
            }
            ViewBag.Category = categories;

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (var country in await _countryRepository.GetCountries())
            {
                countries.Add(new SelectListItem() { Value = country.CountryName, Text = country.CountryName });
            }
            ViewBag.CreatedPlace = countries;
            ViewBag.StatusMessage = StatusMessage;

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,Brand,PriceForOneKilogram,Price,Weight,Protein,Fat,Carbohydrates,EnergyValue,IsDeleted,InStock,Quantity,ImageUrl,AddedDate,CreatedPlace,Category")] Product product, string createdPlace, string category)
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            foreach (var value in await _categoryRepository.GetCategories())
            {
                categories.Add(new SelectListItem() { Value = value.CategoryName, Text = value.CategoryName });
            }
            ViewBag.Category = categories;

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (var country in await _countryRepository.GetCountries())
            {
                countries.Add(new SelectListItem() { Value = country.CountryName, Text = country.CountryName });
            }
            ViewBag.CreatedPlace = countries;

            if (ModelState.IsValid)
            {
                if (category == null)
                {
                    StatusMessage = "Error. Category doesn't choosen.";
                    return RedirectToAction();
                }
                Category temporalCategory = await _categoryRepository.FindFirsByCategoryName(category);

                if (createdPlace == null)
                {
                    StatusMessage = "Error. Coountry doesn't choosen.";
                    return RedirectToAction();
                }
                Country temporalCountry = await _countryRepository.FindFirsByCountryName(createdPlace);

                if (temporalCategory != null && temporalCountry != null)
                {
                    product.Category = temporalCategory;
                    product.CreatedPlace = temporalCountry;
                }
                else
                {
                    StatusMessage = "Not all properties choosen";
                    return RedirectToAction();
                }
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var imagePath = await _imageService.SaveImageAsync(file);
                    product.ProductPicture = imagePath;
                    product.ImageName = file.FileName;
                }
                else
                {
                    StatusMessage = "Error. Image doesn't choosen.";
                    return RedirectToAction();
                }
                _productRepository.InsertProduct(product);
                StatusMessage = "Product has been added";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(product);
        }

        // GET: Products/Edit/5
        [Route("Edit/{id:long:min(1)?}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByID(id);
            if (product == null)
            {
                return NotFound();
            }

            List<SelectListItem> categories = new List<SelectListItem>();
            foreach (var category in await _categoryRepository.GetCategories())
            {
                categories.Add(new SelectListItem() { Value = category.CategoryName, Text = category.CategoryName });
            }
            ViewBag.Category = categories;

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (var country in await _countryRepository.GetCountries())
            {
                countries.Add(new SelectListItem() { Value = country.CountryName, Text = country.CountryName });
            }
            ViewBag.CreatedPlace = countries;

            ViewBag.StatusMessage = StatusMessage;
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:long:min(1)}")]
        public async Task<IActionResult> Edit(long id, [Bind("ProductId,ProductName,ProductDescription,Brand,PriceForOneKilogram,Price,Weight,Protein,Fat,Carbohydrates,EnergyValue,IsDeleted,InStock,Quantity,ImageUrl,AddedDate")] Product product, string createdPlace, string category)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            List<SelectListItem> categories = new List<SelectListItem>();
            foreach (var value in await _categoryRepository.GetCategories())
            {
                categories.Add(new SelectListItem() { Value = value.CategoryName, Text = value.CategoryName });
            }
            ViewBag.Category = categories;

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (var country in await _countryRepository.GetCountries())
            {
                countries.Add(new SelectListItem() { Value = country.CountryName, Text = country.CountryName });
            }
            ViewBag.CreatedPlace = countries;

            if (ModelState.IsValid)
            {
                Category temporalCategory = await _categoryRepository.FindFirsByCategoryName(category);
                Country temporalCountry = await _countryRepository.FindFirsByCountryName(createdPlace);
                if (temporalCategory != null && temporalCountry != null)
                {
                    product.Category = temporalCategory;
                    product.CreatedPlace = temporalCountry;
                }
                else
                {
                    StatusMessage = "Not all properties choosen";
                    return RedirectToAction();
                }

                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var imagePath = await _imageService.SaveImageAsync(file);
                    product.ProductPicture = imagePath;
                    product.ImageName = file.FileName;
                }
                else
                {
                    StatusMessage = "Error. Image doesn't choosen.";
                    return RedirectToAction();
                }

                try
                {
                    _productRepository.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Product has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Invalid form.";
            return View(product);
        }

        // GET: Products/Delete/5
        [Route("Delete/{id:long:min(1)?}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByID(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:long:min(1)}")]
        public IActionResult DeleteConfirmed(long id)
        {
            _productRepository.DeleteProduct(id);
            StatusMessage = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(long id)
        {
            return _productRepository.ProductExist(id);
        }
    }
}
