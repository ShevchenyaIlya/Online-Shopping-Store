using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using ProductStore.Repositories;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("Admin/Categories")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ImageService _imageService;

        [TempData]
        public string StatusMessage { get; set; }

        public CategoriesController(ICategoryRepository categoryRepository, ImageService imageService)
        {
            _categoryRepository = categoryRepository;
            _imageService = imageService;
        }

        // GET: Categories
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _categoryRepository.GetCategories());
        }

        // GET: Categories/Details/5
        [Route("Details/{id:custom?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategorytByID(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(category);
        }

        // GET: Categories/Create
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryDescription,IsDeleted,ImageUrl,AddedDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var path = await _imageService.SaveImageAsync(file);
                    category.CategoryPicture = path;
                    category.ImageName = file.FileName;
                }
                else
                {
                    StatusMessage = "Error. Image doesn't choosen.";
                    return RedirectToAction();
                }
                _categoryRepository.InsertCategory(category);
                StatusMessage = "Category has been created";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Form is invalid";
            return View(category);
        }

        // GET: Categories/Edit/5
        [Route("Edit/{id:custom?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategorytByID(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:custom}")]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription,IsDeleted,ImageUrl,AddedDate")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var path = await _imageService.SaveImageAsync(file);
                    category.CategoryPicture = path;
                    category.ImageName = file.FileName;
                }
                else
                {
                    StatusMessage = "Error. Image doesn't choosen.";
                    return RedirectToAction();
                }

                try
                {
                    _categoryRepository.UpdateCategory(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Category has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Form is invalid";
            return View(category);
        }

        // GET: Categories/Delete/5
        [Route("Delete/{id:custom?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategorytByID(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.StatusMessage = StatusMessage;
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:custom}")]
        public IActionResult DeleteConfirmed(int id)
        {
            _categoryRepository.RemoveCategory(id);
            StatusMessage = "Category has been deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _categoryRepository.CategoryExist(id);
        }
    }
}
