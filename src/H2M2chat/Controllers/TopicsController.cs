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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace H2M2chat.Controllers
{
    public class TopicsController : Controller
    {
        private readonly AppDbContext _context;

        public TopicsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Topics
        public async Task<IActionResult> Index(string search,string tag)
        {
            var Topics = from m in _context.Topic
                         select m;
            if (!String.IsNullOrEmpty(search))
            {
                Topics = Topics.Where(s => s.Title!.Contains(search));
            }
            if (!String.IsNullOrEmpty(tag))
            {
                Topics = Topics.Where(s => s.Tags!.Contains(tag));
            }

            return View(await Topics.ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

       /* // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }*/

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Tags")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.TopicId = Guid.NewGuid();
                topic.Title = System.Net.WebUtility.HtmlEncode(topic.Title);
                topic.Description = System.Net.WebUtility.HtmlEncode(topic.Description);
                topic.Tags = System.Net.WebUtility.UrlEncode(topic.Tags);
                topic.Creator = User.Identity.Name;
                _context.Add(topic);
                await _context.SaveChangesAsync();
                Console.WriteLine(topic.TopicId);
                return Redirect($"~/Topics/Details/{topic.TopicId}");
            }
            Console.WriteLine(ModelState.Values.SelectMany(v => v.Errors));
            return Redirect("~/");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment([Bind("TopicId,ParentId,Message")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CommentId = Guid.NewGuid();
                comment.Creator = User.Identity.Name;
                comment.Message = System.Net.WebUtility.UrlEncode(comment.Message);
                if (comment.ParentId == Guid.Empty)
                {
                    var topic = await _context.Topic
                    .FirstOrDefaultAsync(m => m.TopicId == comment.TopicId);
                    topic.Comments.Add(comment);
                    _context.Update(topic);
                    
                }
                else
                {
                    var subcomment = await _context.Comment
                   .FirstOrDefaultAsync(m => m.CommentId == comment.ParentId);
                    comment.SubComments.Add(subcomment);
                    _context.Update(comment);

                }
                await _context.SaveChangesAsync();
                return Redirect($"~/Topics/Details/{comment.TopicId}");

            }

            Console.WriteLine(ModelState.Values.SelectMany(v => v.Errors));
            return Redirect("~/");
        }


        /*// GET: Topics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TopicId,Title,Creator,Description,Tags,Created")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var topic = await _context.Topic.FindAsync(id);
            _context.Topic.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool TopicExists(Guid id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }
    }
}
