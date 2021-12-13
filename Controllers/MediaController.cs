using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace.Data;
using GroupSpace.Models;
using Microsoft.AspNetCore.Authorization;

namespace GroupSpace.Controllers
{ 
    [Authorize]
    public class MediaController : ApplicationController
    {
        public MediaController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger)
            : base(context, httpContextAccessor, logger)
        {
        }

        // GET: Media
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var groupSpaceContext = _context.Media.Include(m => m.Type)
                                            .Include(m => m.Categories);
            return View(await groupSpaceContext.ToListAsync());
        }

        // GET: Media/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var media = await _context.Media
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // GET: Media/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.MediaType, "Id", "Denominator");
            ViewData["CategoryIds"] = new MultiSelectList(_context.Category.OrderBy(c => c.Name), "Id", "Name");
            Media media = new Media { Added = DateTime.Now };
            return View(media);
        }

        // POST: Media/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Added,TypeId,CategorieIds")] Media media)
        {
            if (ModelState.IsValid)
            {
                if (media.Categories == null)
                    media.Categories = new List<Category>();
                foreach (int id in media.CategorieIds)
                    media.Categories.Add(_context.Category.FirstOrDefault(c => c.Id == id));
                _context.Add(media);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.MediaType, "Id", "Denominator", media.TypeId);
            ViewData["CategoryIds"] = new MultiSelectList(_context.Category.OrderBy(c => c.Name), "Id", "Name");
            return View(media);
        }

        // GET: Media/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var media = await _context.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }
            media.CategorieIds = new List<int>();
            if (media.Categories != null)
                foreach (Category cat in media.Categories)
                    media.CategorieIds.Add(cat.Id);
            ViewData["TypeId"] = new SelectList(_context.MediaType, "Id", "Name", media.TypeId);
            ViewData["CategoryIds"] = new MultiSelectList(_context.Category.OrderBy(c => c.Name), "Id", "Name",media.CategorieIds);
            return View(media);
        }

        // POST: Media/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Added,TypeId,CategorieIds")] Media media)
        {
            if (id != media.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (media.Categories == null)
                        media.Categories = new List<Category>();
                    foreach (int i in media.CategorieIds)
                        media.Categories.Add(_context.Category.FirstOrDefault(c => c.Id == i));
                    _context.Update(media);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaExists(media.Id))
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
            media.CategorieIds = new List<int>();
            if (media.Categories != null)
                foreach (Category cat in media.Categories)
                    media.CategorieIds.Add(cat.Id);
            ViewData["TypeId"] = new SelectList(_context.MediaType, "Id", "Name", media.TypeId);
            ViewData["CategoryIds"] = new MultiSelectList(_context.Category.OrderBy(c => c.Name), "Id", "Name", media.CategorieIds);
            return View(media);
        }

        // GET: Media/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var media = await _context.Media
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var media = await _context.Media.FindAsync(id);
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MediaExists(int id)
        {
            return _context.Media.Any(e => e.Id == id);
        }
    }
}
