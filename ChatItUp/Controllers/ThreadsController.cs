using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChatItUp.Data;
using ChatItUp.Models;
using ChatItUp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ChatItUp.Controllers
{
    public class ThreadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThreadsController(ApplicationDbContext context, UserManager<ApplicationUser> user)
        {
            _context = context;
            _userManager = user;
        }


        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Threads
        public async Task<IActionResult> Index(int? id)
        {
            ThreadForumViewModel ThreadVM = new ThreadForumViewModel();
            if(id == null)
            {
                return NotFound();
            }

           ThreadVM.Thread =await _context.Thread.Include(t => t.Forum).Where(f => f.ForumId == (int)id).ToListAsync();
            ThreadVM.Forum = _context.Forum.Include(t => t.thread).SingleOrDefault(m => m.ForumId == (int)id);

            return View(ThreadVM);
        }

        // GET: Threads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thread = await _context.Thread
                .Include(t => t.Forum)
                .SingleOrDefaultAsync(m => m.ThreadId == id);
            if (thread == null)
            {
                return NotFound();
            }

            return View(thread);
        }

        // GET: Threads/Create
        [Authorize]
        public async Task <IActionResult> Create(int? id)
        {
            var thread = new Thread();
            thread.ForumId = (int)id;
            var user = await GetCurrentUserAsync();
            ViewData["ForumId"] = new SelectList(_context.Forum, "ForumId", "ForumId");
            return View(thread);
        }

        // POST: Threads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Thread thread)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
               
                var user = await GetCurrentUserAsync();
                thread.User = user;
                _context.Add(thread);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new {id= thread.ForumId });
            }
            ViewData["ForumId"] = new SelectList(_context.Forum, "ForumId", "ForumId", thread.Forum.ForumId);
            ThreadForumViewModel ThreadVM2 = new ThreadForumViewModel();
            return View(thread);
        }

        // GET: Threads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thread = await _context.Thread.SingleOrDefaultAsync(m => m.ThreadId == id);
            if (thread == null)
            {
                return NotFound();
            }
            ViewData["ForumId"] = new SelectList(_context.Forum, "ForumId", "ForumId", thread.ForumId);
            return View(thread);
        }

        // POST: Threads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThreadId,Title,ForumId,created")] Thread thread)
        {
            if (id != thread.ThreadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thread);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThreadExists(thread.ThreadId))
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
            ViewData["ForumId"] = new SelectList(_context.Forum, "ForumId", "ForumId", thread.ForumId);
            return View(thread);
        }

        // GET: Threads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thread = await _context.Thread
                .Include(t => t.Forum)
                .SingleOrDefaultAsync(m => m.ThreadId == id);
            if (thread == null)
            {
                return NotFound();
            }

            return View(thread);
        }

        // POST: Threads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thread = await _context.Thread.SingleOrDefaultAsync(m => m.ThreadId == id);
            _context.Thread.Remove(thread);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ThreadExists(int id)
        {
            return _context.Thread.Any(e => e.ThreadId == id);
        }
    }
}
