#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using H2M2chat.Data;
using H2M2chat.Models;
using Microsoft.AspNetCore.Authorization;

namespace H2M2chat.Controllers
{
    public class ChatRoomController : Controller
    {
        private readonly AppDbContext _context;

        public ChatRoomController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ChatRoom
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Room.ToListAsync());
        }
        [Authorize]
        // GET: ChatRoom/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        [Authorize]
        // GET: ChatRoom/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatRoom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("About,IsGC,IsVisable")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.RoomId = Guid.NewGuid();
                room.RoomIcon = Guid.NewGuid();
                _context.Add(room);
                await _context.SaveChangesAsync();
                return Redirect($"~/ChatRoom/Details/{room.RoomId}");
            }
            return View(room);
        }

        // GET: ChatRoom/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: ChatRoom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, [Bind("RoomId,About,IsGC,IsVisable")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"~/ChatRoom/Details/{room.RoomId}");
            }
            return View(room);
        }

        // GET: ChatRoom/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: ChatRoom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(Guid id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }
    }
}
