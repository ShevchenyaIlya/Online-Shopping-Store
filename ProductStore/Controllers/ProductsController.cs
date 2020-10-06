﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AuthDbContext _context;

        public ProductsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            foreach (var category in _context.Category)
            {
                categories.Add(new SelectListItem() { Value = category.CategoryName, Text = category.CategoryName });
            }
            ViewBag.Category = categories;

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (var country in _context.Country)
            {
                countries.Add(new SelectListItem() { Value = country.CountryName, Text = country.CountryName });
            }
            ViewBag.CreatedPlace = countries;
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,Brand,PriceForOneKilogram,Price,Weight,Protein,Fat,Carbohydrates,EnergyValue,IsDeleted,InStock,Quantity,ImageUrl,AddedDate,CreatedPlace,Category")] Product product, string createdPlace, string category)
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            foreach (var value in _context.Category)
            {
                categories.Add(new SelectListItem() { Value = value.CategoryName, Text = value.CategoryName });
            }
            ViewBag.Category = categories;

            List<SelectListItem> countries = new List<SelectListItem>();
            foreach (var country in _context.Country)
            {
                countries.Add(new SelectListItem() { Value = country.CountryName, Text = country.CountryName });
            }
            ViewBag.CreatedPlace = countries;

            if (ModelState.IsValid)
            {
                Category temporalCategory = _context.Category.Where(value => value.CategoryName == category).First();
                Country temporalCountry = _context.Country.Where(value => value.CountryName == createdPlace).First();
                if (temporalCategory != null && temporalCountry != null)
                {
                    product.Category = temporalCategory;
                    product.CreatedPlace = temporalCountry;
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProductId,ProductName,ProductDescription,Brand,PriceForOneKilogram,Price,Weight,Protein,Fat,Carbohydrates,EnergyValue,IsDeleted,InStock,Quantity,ImageUrl,AddedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
