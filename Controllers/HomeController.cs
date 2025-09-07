using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingList.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ShoppingList.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MydatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, MydatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        private string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
        
        public IActionResult Index(int? page, string filter)
        {
            if(filter == "‚·‚×‚Ä")
                filter = null;

            if (page == null)
                page = 0;

            int max = 10;
            ViewData["Filter"] = filter;
            var userId = UserId;
            if (userId == null)
                return RedirectToAction("Error");

            var items = _context.ShoppingLists.Where(s => s.UserId == userId).ToList();
            ViewData["Shops"] = items.GroupBy(s => s.ShopName).Select(g => g.First().ShopName).ToList();

            items = items.Skip(page.Value * max).Take(max).ToList();

            if (filter != null)
                items = items.Where(s => s.ShopName == filter).ToList();

            items.Skip(max * (page.Value + 1)).Take(max);
            if (page > 0)
                ViewData["Prev"] = page.Value - 1;
            if (items.Count >= max)
            {
                ViewData["Next"] = page.Value + 1;
                if(_context.ShoppingLists.Skip(max * (page.Value + 1)).Take(max).Count() == 0)
                    ViewData["Next"] = null;
                
                items.Skip(max * (page.Value + 1)).Take(max);
            }
            

            return View(items);
        }
        public IActionResult Add()
        {
            return View();
        }
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var userId = UserId;
        //    if (userId == null)
        //        return RedirectToAction("Error");

        //    var shoppingList = await _context.ShoppingLists.FindAsync(id);
        //    if (shoppingList == null || shoppingList.UserId != userId)
        //        return NotFound();
        //    return View(shoppingList);
        //}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = UserId;
            if (userId == null)
                return RedirectToAction("Error");

            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList == null || shoppingList.UserId != userId)
                return NotFound();

            return View(shoppingList);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string ItemName, string ShopName, [Bind("Id,UserId,ShopName,ItemName")]Models.ShoppingList ShopItem)
        {
            var userId = UserId;
            if (userId == null)
                return RedirectToAction("Error");

            ShopItem.UserId = userId;
            _context.Add(ShopItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
                return NotFound();

            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList != null)
            {
                _context.ShoppingLists.Remove(shoppingList);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ShopName,ItemName")] Models.ShoppingList shoppingList)
        {
            if(id != shoppingList.Id)
                return NotFound();
            var userId = UserId;
            if (userId == null)
                return RedirectToAction("Error");
            shoppingList.UserId = userId;
            try
            {
                _context.Update(shoppingList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ShoppingLists.Any(e => e.Id == shoppingList.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
