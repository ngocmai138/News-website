using Microsoft.AspNetCore.Mvc;
using News.Data;

namespace News.Components
{
    public class Footer:ViewComponent
    {
        private readonly NewsContext _context;
        public Footer(NewsContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Category.ToList());
        }
    }
}
