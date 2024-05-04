using Microsoft.AspNetCore.Mvc;
using News.Data;

namespace News.Models
{
    public class Navbar:ViewComponent
    {
        private readonly NewsContext _context;

        public Navbar(NewsContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() 
        {  
            return View(_context.Category.ToList()); 
        }
    }
}
