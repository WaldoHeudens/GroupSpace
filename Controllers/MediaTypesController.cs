﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace.Data;
using GroupSpace.Models;

namespace GroupSpace.Controllers
{
    public class MediaTypesController : Controller
    {
        private readonly GroupSpaceContext _context;

        public MediaTypesController(GroupSpaceContext context)
        {
            _context = context;
        }

        // GET: MediaTypes
        public async Task<IActionResult> Index()
        {
            var media = _context.Media
                                    .Include(m => m.Type)
                                    .Include(m => m.Categories)
                                    .OrderBy(m => m.Name);
            return View(await media.ToListAsync());
        }

        // GET: MediaTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaType = await _context.MediaType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mediaType == null)
            {
                return NotFound();
            }

            return View(mediaType);
        }

        // GET: MediaTypes/Create
        public IActionResult Create()
        {
            MediaType mt = new MediaType { Deleted = DateTime.MaxValue };
            return View(mt);
        }

        // POST: MediaTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Denominator,Deleted")] MediaType mediaType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mediaType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mediaType);
        }

        // GET: MediaTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaType = await _context.MediaType.FindAsync(id);
            if (mediaType == null)
            {
                return NotFound();
            }
            return View(mediaType);
        }

        // POST: MediaTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Denominator,Deleted")] MediaType mediaType)
        {
            if (id != mediaType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mediaType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaTypeExists(mediaType.Id))
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
            return View(mediaType);
        }

        // GET: MediaTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediaType = await _context.MediaType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mediaType == null)
            {
                return NotFound();
            }

            return View(mediaType);
        }

        // POST: MediaTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediaType = await _context.MediaType.FindAsync(id);
            _context.MediaType.Remove(mediaType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MediaTypeExists(int id)
        {
            return _context.MediaType.Any(e => e.Id == id);
        }
    }
}
