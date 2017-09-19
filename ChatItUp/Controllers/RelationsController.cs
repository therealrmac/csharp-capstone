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
    public class RelationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RelationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Relations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Relation.ToListAsync());
        }

        //GET: USER PROFILE
        public async Task<IActionResult> UserProfile(string id)
        {
            UserProfileViewModel upVM = new UserProfileViewModel();
            var user = await GetCurrentUserAsync();
            if(id == null)
            {
                return NotFound();
            }
            if(id == user.Id)
            {
                return RedirectToAction("Index", "Manage");
            }
            var userFriend = await _context.ApplicationUser.SingleOrDefaultAsync(x => x.Id == id);

            var CheckConnectedButNotConfirmed =  _context.Relation.Any(x => x.Friend == userFriend &&  x.User.Id == user.Id && x.Connected == null);

            var CheckConnectedAndConfirmed = _context.Relation.Any(x => x.Friend == userFriend && x.User.Id == user.Id && x.Connected == true);

            var CheckConnectedAndConfirmed2 = _context.Relation.Any(x => x.User == userFriend && x.Friend.Id == user.Id && x.Connected == true);
            var completeFriendList = await _context.Relation.Include("Friend").Where(x => x.User == user && x.Friend.Id==id && x.Connected == true).ToListAsync();
            var completeFriendList2 = await _context.Relation.Include("User").Where(x => x.Friend == user && x.User.Id==id && x.Connected == true).ToListAsync();
            var completeFriendList3 = await _context.Relation.Include("Friend").Where(x => x.User.Id == id && x.User!= user && x.Friend != user && x.Connected == true).ToListAsync();
            var completeFriendList4 = await _context.Relation.Include("User").Where(x => x.Friend.Id == id && x.User != user  && x.Friend != user && x.Connected == true).ToListAsync();

            var posts = _context.ThreadPost.Where(x => x.user.Id == id);

            var threadsCretaed = await _context.Thread.Where(x => x.User.Id == id).ToListAsync();

            upVM.checkconnectedbutnotconfirmed = CheckConnectedButNotConfirmed;
            upVM.checkConnectedAndConfirmed = CheckConnectedAndConfirmed;
            upVM.checkConnectedAndConfirmed2 = CheckConnectedAndConfirmed2;
            upVM.User = await _context.ApplicationUser.Where(u => u.Id == id).SingleOrDefaultAsync();
            upVM.friendList = completeFriendList;
            upVM.friendList2 = completeFriendList2;
            upVM.friendList3 = completeFriendList3;
            upVM.friendList4 = completeFriendList4;
            upVM.totalThreads = threadsCretaed;
            upVM.totalPosts = posts;            

            return View(upVM);
        }

        //AddFriend Post
        [HttpPost]
        public async Task<IActionResult> AddFriend(ApplicationUser user)
        {
            
            var currentUser = await GetCurrentUserAsync();
            if(currentUser == null)
            {
                return NotFound();
            }

            ApplicationUser userFriend = await _context.ApplicationUser.Where(u => u.Id == user.Id).SingleOrDefaultAsync();

            var relationB = new Relation() {User = currentUser, Friend = userFriend};



            _context.Add(relationB);
            await _context.SaveChangesAsync();


           return RedirectToAction("UserProfile", new {id= userFriend.Id });
        }



        //DELETEFRIEND POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFriend(string id)
        {
            var currentUser = await GetCurrentUserAsync();
            if(currentUser == null)
            {
                return NotFound();
            }
            ApplicationUser userFriend = await _context.ApplicationUser.SingleOrDefaultAsync(x => x.Id == id);

            try
            {
            var UserIdIsCurrentUser = _context.Relation.Include("Friend").Single(x => x.User == currentUser && x.Friend.Id== id && x.Connected == true);
            if (UserIdIsCurrentUser != null)
            {
                _context.Remove(UserIdIsCurrentUser);
            }
            }
            catch
            {
            var friendIdIsYourFriend = _context.Relation.Single(x => x.User.Id == id && x.Friend== currentUser && x.Connected == true);
            if(friendIdIsYourFriend != null)
            {
                _context.Remove(friendIdIsYourFriend);
            }

            }
            

            

           
            await _context.SaveChangesAsync();
            return RedirectToAction("UserProfile", new { id = userFriend.Id });

        }



        // GET: Relations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relation = await _context.Relation
                .SingleOrDefaultAsync(m => m.RelationshipId == id);
            if (relation == null)
            {
                return NotFound();
            }

            return View(relation);
        }

        // GET: Relations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Relations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RelationshipId,connectedOn,connected")] Relation relation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(relation);
        }

        // GET: Relations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relation = await _context.Relation.SingleOrDefaultAsync(m => m.RelationshipId == id);
            if (relation == null)
            {
                return NotFound();
            }
            return View(relation);
        }

        // POST: Relations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RelationshipId,connectedOn,connected")] Relation relation)
        {
            if (id != relation.RelationshipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelationExists(relation.RelationshipId))
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
            return View(relation);
        }

        // GET: Relations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relation = await _context.Relation
                .SingleOrDefaultAsync(m => m.RelationshipId == id);
            if (relation == null)
            {
                return NotFound();
            }

            return View(relation);
        }

        // POST: Relations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relation = await _context.Relation.SingleOrDefaultAsync(m => m.RelationshipId == id);
            _context.Relation.Remove(relation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RelationExists(int id)
        {
            return _context.Relation.Any(e => e.RelationshipId == id);
        }
    }
}
