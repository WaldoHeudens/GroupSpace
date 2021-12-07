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
using GroupSpace.Areas.Identity.Data;

namespace GroupSpace.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index(string titleFilter, int selectedGroup, string orderBy, string selectedMode = "R")
        {
            // Lijst alle message op.  We gebruiken Linq
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var filteredMessages = from m in _context.Message
                                   where selectedMode == "S" && m.SenderId == user.Id
                                   select m;

            // Pas de groepfilter (selectedGroup) toe als deze niet leeg is
            if (selectedGroup != 0)
                filteredMessages = from m in filteredMessages where m.GroupID == selectedGroup select m;

            // Pas de titleFilter toe (als deze niet leeg is) en zorg dat de groep-instanties daaraan toegevoegd zijn en sorteer
            if (!string.IsNullOrEmpty(titleFilter))
                filteredMessages = from m in filteredMessages where m.Title.Contains(titleFilter) select m;

            ViewData["TitleField"] = string.IsNullOrEmpty(orderBy) ? "Titles_Desc" : "";
            ViewData["GroupField"] = orderBy == "Group" ? "Group_Desc" : "Group";

            switch (orderBy)
            {
                case "Group":
                    filteredMessages = filteredMessages.OrderBy(m => m.Group.Name);
                    break;
                case "Group_Desc":
                    filteredMessages = filteredMessages.OrderByDescending(m => m.Group.Name);
                    break;
                case "Titles_Desc":
                    filteredMessages = filteredMessages.OrderByDescending(m => m.Title);
                    break;
                default:
                    filteredMessages = filteredMessages.OrderBy(m => m.Title);
                    break;
            }

            // Lijst van groepen 
            IQueryable<Group> groupsToSelect = from g in _context.Group orderby g.Name select g;

            // Selectieveldje voor de mode van gebruik
            var modeItems = new List<SelectListItem>
            {
                new SelectListItem{Value="R", Text="Ontvangen", Selected = selectedMode == "R" },
                new SelectListItem{Value="S", Text="Verzonden", Selected = selectedMode == "S"}
            };

            // Maak een object van de view-model-class en voeg daarin alle wat we nodig hebben
            MessageIndexViewModel messageIndexViewModel = new MessageIndexViewModel()
            {
                TitleFilter = titleFilter,
                FilteredMessages = await filteredMessages.Include(s=>s.Group).ToListAsync(),
                SelectedGroup = selectedGroup,
                GroupsToSelect = new SelectList(await groupsToSelect.ToListAsync(), "Id", "Name", selectedGroup),
                ModesToSelect = new SelectList(modeItems, "Value", "Text"),
                SelectedMode = selectedMode
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
                message.Sender = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
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
