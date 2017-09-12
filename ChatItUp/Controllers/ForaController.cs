using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChatItUp.Data;
using ChatItUp.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatItUp.Controllers
{
    public class ForaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForaController(ApplicationDbContext context, UserManager<ApplicationUser> user)
        {
            _context = context;
            _userManager = user;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Fora
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Forum.Include(f => f.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: Forum/index/5
        public async Task<IActionResult> Index2(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var currentUser = await GetCurrentUserAsync();
            var forum = await _context.Forum.Where(f => f.CategoryId == id).ToListAsync();
            if(forum == null)
            {
                return NotFound();
            }
            return View(forum);
        }

        // GET: Fora/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum
                .Include(f => f.Category)
                .SingleOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // GET: Fora/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Fora/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForumId,ThreadTitles,CategoryId")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", forum.CategoryId);
            return View(forum);
        }

        // GET: Fora/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum.SingleOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", forum.CategoryId);
            return View(forum);
        }

        // POST: Fora/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ForumId,ThreadTitles,CategoryId")] Forum forum)
        {
            if (id != forum.ForumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumExists(forum.ForumId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", forum.CategoryId);
            return View(forum);
        }

        // GET: Fora/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum
                .Include(f => f.Category)
                .SingleOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // POST: Fora/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forum = await _context.Forum.SingleOrDefaultAsync(m => m.ForumId == id);
            _context.Forum.Remove(forum);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ForumExists(int id)
        {
            return _context.Forum.Any(e => e.ForumId == id);
        }
    }
}
