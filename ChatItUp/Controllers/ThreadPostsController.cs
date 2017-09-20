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
using ChatItUp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ChatItUp.Controllers
{
    public class ThreadPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThreadPostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: ThreadPosts
        public async Task<IActionResult> Index(int? id)
        {
            ThreadPostIndexViewModel ThreadPostVM = new ThreadPostIndexViewModel();

            if(id == null)
            {
                return NotFound();
            }
            ThreadPostVM.threadpost = await _context.ThreadPost.Include(t => t.Thread).Include(u => u.user).Where(tp => tp.ThreadId == (int)id).ToListAsync();

            ThreadPostVM.Thread =  _context.Thread.Include(t => t.ThreadPost).Include(u => u.User).SingleOrDefault(tp => tp.ThreadId == (int)id);


            ThreadPostVM.Forum = _context.Forum.Include(t => t.thread).SingleOrDefault(m => m.ForumId == ThreadPostVM.Thread.ForumId);
            

            return View(ThreadPostVM);
        }

        // GET: ThreadPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threadPost = await _context.ThreadPost
                .Include(t => t.Thread)
                .SingleOrDefaultAsync(m => m.ThreadPostId == id);
            if (threadPost == null)
            {
                return NotFound();
            }

            return View(threadPost);
        }

        // GET: ThreadPosts/Create
        [Authorize]
        public async Task<IActionResult> Create(int? id)
        {
            var threadpost = new ThreadPost();
            threadpost.ThreadId = (int)id;

            var thread= await _context.Thread
                .Where(t => t.ThreadId == threadpost.ThreadId).ToListAsync();

            var user = await GetCurrentUserAsync();
            ViewData["ThreadId"] = new SelectList(_context.Thread, "ThreadId", "ThreadId");
            return View(threadpost);
        }

        // POST: ThreadPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ThreadPost threadPost)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                threadPost.user = user;
                _context.Add(threadPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = threadPost.ThreadId});
            }
            ViewData["ThreadId"] = new SelectList(_context.Thread, "ThreadId", "ThreadId", threadPost.ThreadId);
            return View(threadPost);
        }

        // GET: ThreadPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threadPost = await _context.ThreadPost.SingleOrDefaultAsync(m => m.ThreadPostId == id);
            if (threadPost == null)
            {
                return NotFound();
            }
            ViewData["ThreadId"] = new SelectList(_context.Thread, "ThreadId", "ThreadId", threadPost.ThreadId);
            return View(threadPost);
        }

        // POST: ThreadPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThreadPostId,message,dateCreatd,ThreadId")] ThreadPost threadPost)
        {
            if (id != threadPost.ThreadPostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(threadPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThreadPostExists(threadPost.ThreadPostId))
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
            ViewData["ThreadId"] = new SelectList(_context.Thread, "ThreadId", "ThreadId", threadPost.ThreadId);
            return View(threadPost);
        }

        // GET: ThreadPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var threadPost = await _context.ThreadPost
                .Include(t => t.Thread)
                .SingleOrDefaultAsync(m => m.ThreadPostId == id);
            if (threadPost == null)
            {
                return NotFound();
            }

            return View(threadPost);
        }

        // POST: ThreadPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var threadPost = await _context.ThreadPost.SingleOrDefaultAsync(m => m.ThreadPostId == id);
            _context.ThreadPost.Remove(threadPost);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ThreadPostExists(int id)
        {
            return _context.ThreadPost.Any(e => e.ThreadPostId == id);
        }
    }
}
