using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLshortener.Domain;

namespace URLshortener.Controllers
{
    public class RedirectController : UrlShortenerBaseController
    {
        private readonly UrlShortenerContext _context;

        public RedirectController(UrlShortenerContext context)
        {
            _context = context;
        }

        // GET: RedirectController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RedirectController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RedirectController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RedirectController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RedirectController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RedirectController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: RedirectController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> RedirectShortUrl(string shortUrl)
        {
            var entry = await _context.UrlEntries.FirstOrDefaultAsync(e => e.ShortURL == shortUrl);
            if (entry == null)
            {
                //Такой ссылки не существует. Редирект на главную.
                return NotFound();
            }
            entry.Clicks++;
            _context.Update(entry);
            await _context.SaveChangesAsync();

            return Redirect(entry.LongURL);
        }
    }
}
