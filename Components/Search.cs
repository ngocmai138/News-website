using Microsoft.AspNetCore.Mvc;
using News.Data;

namespace News.Components
{
    public class Search:ViewComponent
    {
        private readonly NewsContext _context;
        public Search(NewsContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Article.ToList());
        }
    }
}
