using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using asp__example.DAL;
using asp__example.Models;

namespace asp__example.Controllers
{
    public class FriendsController : Controller
    {
        private readonly DataContext _context;

        public FriendsController(DataContext context)
        {
            _context = context;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            var res = await _context.Friends.Include(f => f.Images).ToListAsync();
            return View(res);
        }

        // GET: Friends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends.Include(f => f.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // GET: Friends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailAddress,Phone,setAvatar , setImges")]  Friend friend)
        {
            if (ModelState.IsValid)
            {
                if(friend.Avatar == null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory() + @"\wwwroot\images\default_avatar.png");
                    friend.Avatar = System.IO.File.ReadAllBytes(path);
                }
                _context.Add(friend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(friend);
        }

        // GET: Friends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends.Include(f => f.Images).FirstOrDefaultAsync(f => f.Id == id);
            if (friend == null)
            {
                return NotFound();
            }
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,EmailAddress,Phone,setImges")] Friend friend, string? imagesToRemove)
        {
            if (id != friend.Id) return NotFound();
            var original = _context.Friends.Include(f => f.Images).FirstOrDefault(f => f.Id == id);
            if (ModelState.IsValid)
            {
                if (original != null)
                {
                    try
                    {
                        original.FirstName = friend.FirstName;
                        original.LastName = friend.LastName;
                        original.Phone = friend.Phone;
                        original.EmailAddress = friend.EmailAddress;

                        if(!string.IsNullOrEmpty(imagesToRemove))
                        {
                            var idsToRemove = imagesToRemove.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(id => int.TryParse(id, out int result)  ? result : (int?)null)
                                .Where(id => id.HasValue)
                                .Select(id => id.Value)
                                .ToList();
                            original.Images = original.Images.Where(i => !idsToRemove.Contains(i.Id)).ToList();
                        }
                        if(friend.Images != null && friend.Images.Any()) original.Images.AddRange(friend.Images);
                        _context.Update(original);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FriendExists(original.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return View(original);
        }

        // GET: Friends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends
                .FirstOrDefaultAsync(m => m.Id == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend != null)
            {
                _context.Friends.Remove(friend);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendExists(int id)
        {
            return _context.Friends.Any(e => e.Id == id);
        }
    }
}
