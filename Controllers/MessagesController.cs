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
    public class MessagesController : Controller
    {
        private readonly GroupSpaceContext _context;

        public MessagesController(GroupSpaceContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index(string titleFilter, int selectedGroup)
        {
            // Lijst alle message op.  We gebruiken Linq
            var filteredMessages = from m in _context.Message select m;

            // Pas de groepfilter (selectedGroup) toe als deze niet leeg is
            if (selectedGroup != 0)
                filteredMessages = from m in filteredMessages where m.GroupID == selectedGroup select m;

            // Pas de titleFilter toe (als deze niet leeg is) en zorg dat de groep-instanties daaraan toegevoegd zijn en sorteer
            if (!string.IsNullOrEmpty(titleFilter))
                filteredMessages = from m in filteredMessages where m.Title.Contains(titleFilter) select m;

            // Lijst van groepen 
            IQueryable<Group> groupsToSelect = from g in _context.Group orderby g.Name select g;

            // Maak een object van de view-model-class en voeg daarin alle wat we nodig hebben
            MessageIndexViewModel messageIndexViewModel = new MessageIndexViewModel()
            {
                TitleFilter = titleFilter,
                FilteredMessages = await filteredMessages.Include(s=>s.Group).ToListAsync(),
                SelectedGroup = selectedGroup,
                GroupsToSelect = new SelectList(await groupsToSelect.ToListAsync(), "Id", "Name", selectedGroup)
            };
            return View(messageIndexViewModel);
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            Message message = new Message() { Created = DateTime.Now };
            ViewData["GroupID"] = new SelectList(_context.Group, "Id", "Name");
            return View(message);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Content,Created,GroupID")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupID"] = new SelectList(_context.Group, "Id", "Name", message.GroupID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["GroupID"] = new SelectList(_context.Group, "Id", "Name", message.GroupID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Content,Created,GroupID")] Message message)
        {
            if (id != message.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.ID))
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
            ViewData["GroupID"] = new SelectList(_context.Group, "Id", "Name", message.GroupID);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.ID == id);
        }
    }
}