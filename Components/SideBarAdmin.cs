using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using News.Data;
using News.Models;
using System.Security.Claims;

namespace News.Components
{
    public class SideBarAdmin:ViewComponent
    {
        private readonly NewsContext _context;
        public SideBarAdmin(NewsContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke() {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
            var user = _context.User.FirstOrDefault(u => u.Name == userIdClaim.Value);
            return View(user);
        }
    }
}
