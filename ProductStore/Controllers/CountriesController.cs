﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("Admin/Countries")]
    [Route("Admin/Country")]
    public class CountriesController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly ImageService _imageService;

        [TempData]
        public string StatusMessage { get; set; }

        public CountriesController(AuthDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Countries
        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View(await _context.Country.ToListAsync());
        }

        // GET: Countries/Details/5
        [Route("Details/{id:int:min(1)?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;

            return View(country);
        }

        // GET: Countries/Create
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.StatusMessage = StatusMessage;
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var path = await _imageService.SaveImageAsync(file);
                    country.CountryPicture = path;
                }
                else
                {
                    StatusMessage = "Error. Image doesn't choosen.";
                    return RedirectToAction();
                }

                _context.Add(country);
                await _context.SaveChangesAsync();
                StatusMessage = "Country has been created";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Error. Form is invalid";
            return View(country);
        }

        // GET: Countries/Edit/5
        [Route("Edit/{id:int:min(1)?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;

            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id:int:min(1)}")]
        public async Task<IActionResult> Edit(int id, [Bind("CountryId,CountryName,CountryAbbreviation,CountryCode")] Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    var path = await _imageService.SaveImageAsync(file);
                    country.CountryPicture = path;
                }
                else
                {
                    StatusMessage = "Error. Image doesn't choosen.";
                    return RedirectToAction();
                }

                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Country has been updated";
                return RedirectToAction(nameof(Index));
            }
            StatusMessage = "Form is invalid";
            return View(country);
        }

        // GET: Countries/Delete/5
        [Route("Delete/{id:int:min(1)?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (country == null)
            {
                return NotFound();
            }
            ViewBag.StatusMessage = StatusMessage;

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id:int:min(1)}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            StatusMessage = "Product has been deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Country.Any(e => e.CountryId == id);
        }
    }
}
