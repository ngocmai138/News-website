using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;

namespace News.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly NewsContext _context;
        private IWebHostEnvironment _env;

        public ArticlesController(NewsContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var newsContext = _context.Article.Include(a => a.Category).Include(a => a.User);
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
            if (roleClaim != null && (roleClaim.Value == "Administrator" || roleClaim.Value == "Author"))
            {
                var sortedNewsContext = newsContext.OrderBy(a => a.IsCensored);
                return View("AdminIndex", await sortedNewsContext.ToListAsync());
            }
            else
            {
                var newsContext2 = newsContext.Where(a => a.IsCensored == 1);
                var sortedNewsContext2 = newsContext2.OrderByDescending(a => a.DateCreated);
                return View("UserIndex", await sortedNewsContext2.ToListAsync());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var newsContext = _context.Article.Include(a => a.Category).Include(a => a.User).Where(a => a.Title.Contains(keyword));
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);

            if (roleClaim != null && (roleClaim.Value == "Administrator" || roleClaim.Value == "Author"))
            {
                return View("AdminIndex", await newsContext.ToListAsync());
            }
            else
            {
                var newsContext2 = newsContext.Where(a => a.IsCensored == 1);
                return View("UserIndex", await newsContext2.ToListAsync());
            }
        }
        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = await _context.Article
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
            if (article == null)
            {
                return NotFound();
            }
            if (roleClaim != null && (roleClaim.Value == "Administrator" || roleClaim.Value == "Author"))
            {
                return View("AdminDetails", article);
            }
            else
            {
            var previousArticle = await _context.Article
                                        .Where(a => a.Id < id)
                                        .OrderByDescending(a => a.Id)
                                        .FirstOrDefaultAsync();
            var nextArticle = await _context.Article
                                    .Where(a => a.Id > id && a.IsCensored == 1)
                                    .OrderBy(a => a.Id)
                                    .FirstOrDefaultAsync();
            ViewBag.PreviousArticle = previousArticle;
            ViewBag.NextArticle = nextArticle;
                return View("UserDetails", article);
            }
        }

        // GET: Articles/Create
        [Authorize(Roles = "Administrator, Author")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userNameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            ViewData["AuthorName"] = userNameClaim.Value;
            var author = _context.User.FirstOrDefault(x => x.Name == userNameClaim.Value);
            ViewData["AuthorId"] = author.Id;
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Author")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Content,DateCreated,AuthorId,CategoryId,IsFeatured,IsCensored")] Article article, IFormFile ImageUrl, IFormFile ThumbUrl)
        {
            Debug.WriteLine("AAAAAAAAAAAAAAuthor Id: " + article.AuthorId);
            if (ModelState.IsValid)
            {
                //  var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                //  var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //var author = _context.User.FirstOrDefault(x => x.Id == article.AuthorId);
                //article.AuthorId = author.Id;
                // lưu file ảnh vào thư mục chỉ định
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageUrl.FileName);
                    var filePath = Path.Combine(_env.WebRootPath,
                                                "images","thumbs","masonry",
                                                fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }
                    article.ImageUrl = "/images/thumbs/masonry/" + fileName;
                }
                if (ThumbUrl != null && ThumbUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(ThumbUrl.FileName);
                    var filePath = Path.Combine(_env.WebRootPath,
                                                "images","thumbs","masonry/",
                                                fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ThumbUrl.CopyToAsync(stream);
                    }
                    article.ThumbUrl = "/images/thumbs/masonry/" + fileName;
                }
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Debug.WriteLine("Lỗi rồiiiIIIIIIII");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "Administrator, Author")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", article.CategoryId);
            ViewData["AuthorId"] = new SelectList(_context.User, "Id", "Name", article.AuthorId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Author")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Content,DateCreated,ImageUrl,AuthorId,CategoryId,IsFeatured,ThumbUrl,IsCensored")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                _context.Article.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
