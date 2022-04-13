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

        public async Task<IActionResult> Comments()
        {
            

            return View(await _context.Comment.ToListAsync());
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
            var comments = from m in _context.Comment
                           select m;
            comments = comments.Where(s => s.TopicId == id );

            topic.Comments = comments.Where(s => s.level == 0).ToList();
            foreach (var comment in topic.Comments)
            {
                comment.SubComments = comments.Where(s => s.level == 1 && s.ParentId==comment.CommentId).ToList();
                foreach(var subComment in comment.SubComments)
                {
                    subComment.SubComments = comments.Where(s => s.level == 2 && s.ParentId == subComment.CommentId).ToList();
                }
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
                
                return Redirect($"~/Topics/Details/{topic.TopicId}");
            }
            
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
                    comment.level = 0;
                    var topic = await _context.Topic
                    .FirstOrDefaultAsync(m => m.TopicId == comment.TopicId);
                    topic.Comments.Add(comment);
                    
                    _context.Update(topic);
                    
                }
                else
                {
                    var subcomment = await _context.Comment
                   .FirstOrDefaultAsync(m => m.CommentId == comment.ParentId);
                    comment.level = subcomment.level + 1;
                    if (comment.level == 3)
                    {
                        return Redirect($"~/Topics/Details/{comment.TopicId}");
                    }
                    subcomment.SubComments.Add(comment);
                    _context.Update(subcomment);

                }
                _context.Comment.Add(comment);
                await _context.SaveChangesAsync();
                return Redirect($"~/Topics/Details/{comment.TopicId}");

            }

            /*Console.WriteLine(ModelState.Values.SelectMany(v => v.Errors));*/
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
        } */

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? TopicId,String Title,String Description,String Tags)
        {

            
            

            if (TopicId != null)
            {
                var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.TopicId == TopicId);
                topic.Description = System.Net.WebUtility.HtmlEncode(Description);
                topic.Tags = System.Net.WebUtility.HtmlEncode(Tags);
                topic.Title = System.Net.WebUtility.HtmlEncode(Title);
                if (topic.Creator == User.Identity.Name)
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
                    return Redirect($"~/Topics/Details/{topic.TopicId}");
                }
                else
                {
                    return Forbid("Not in my website :3");
                }
                
            }
            return NotFound(); ;
        }
        /*
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
        }*/

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? TopicId)
        {
            if (TopicId == null)
            {
                
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.TopicId == TopicId);

            if (topic == null)
            {
                return NotFound();
            }
            
            if (topic.Creator == User.Identity.Name)
            {
                var comments = from m in _context.Comment
                               select m;
                comments = comments.Where(s => s.TopicId == TopicId);
                _context.Comment.RemoveRange(comments);
                _context.Topic.Remove(topic);
                await _context.SaveChangesAsync();
            }
            else
            {
                return Forbid("Not in my website :3");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(Guid id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }
    }
}
