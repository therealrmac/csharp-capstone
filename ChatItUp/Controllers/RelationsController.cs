using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChatItUp.Data;
using ChatItUp.Models;

namespace ChatItUp.Controllers
{
    public class RelationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelationsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Relations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Relation.ToListAsync());
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
