using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Data;

namespace News.Components
{
    public class FeaturedNews:ViewComponent
    {
        private readonly NewsContext _context;
        public FeaturedNews(NewsContext newsContext)
        {
            this._context = newsContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var newsContext = _context.Article.Include(a => a.Category).Include(a => a.User).Where(a => a.IsFeatured==true);
            var articles = await newsContext.ToListAsync();
            return View(articles);
        }
    }
}
