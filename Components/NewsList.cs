using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Data;

namespace News.Components
{
    public class NewsList:ViewComponent
    {
        private readonly NewsContext _context;
        public NewsList(NewsContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Article.Include(a => a.Category).Include(a => a.User).Where(a =>a.IsCensored ==1).OrderByDescending(a => a.DateCreated).ToList());
        }
    }
}
