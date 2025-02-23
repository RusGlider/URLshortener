using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URLshortener.Domain;
using URLshortener.Models;
using URLshortener.Models.Home;
using URLshortener.Domain.Entities;
using System.Xml;


namespace URLshortener.Controllers;
[Authorize]
public class HomeController : UrlShortenerBaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly UrlShortenerContext _context;

    /*
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    */
    public HomeController(UrlShortenerContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> IndexAsync()
    {
        ViewData["Site"] = GetCurrentUrl();
        return View(new HomeViewModel
        {
            UrlEntries = await GetUrlsCurrentUserAsync()
        });
    }

    public async Task<IActionResult> CreateAsync(HomeViewModel model) //�������� ����� ����������� ������
    {
        if (!ModelState.IsValid)
        {
            model.UrlEntries = await GetUrlsCurrentUserAsync();
            return View("Index", model);
        }

        var urlEntry = await _context.UrlEntries.FirstOrDefaultAsync(url => url.LongURL == model.LongURL);
        if (urlEntry != null)
        {
            //������ ����������. 
            //TODO �������� ������� � ������ ������ ������������ UserId, UrlID
            var userLink = await _context.UserLinks.FirstOrDefaultAsync(u => u.UserId == CurrentUserId && u.UrlId == urlEntry.Id);
            if (userLink != null)
            {
                //������ � ������������ ��� ���������� 
                ViewBag.Error("����� ������ � ������������ ��� ����������.");
                return View("Index", model);
            }
            // ����� ��������� � ������������ ������
            await _context.UserLinks.AddAsync(new UserLink
            {
                UserId = CurrentUserId,
                UrlId = urlEntry.Id
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        //����� ������ �� ������������. ������ ����� ������
        var urlHash = GenerateUrlHash(model.LongURL);
        var domain = GetCurrentUrl(); //TODO ������� ������� ����� ���� https://localhost:55555/ 
        var shortURL = domain + urlHash;


        var newEntry = new UrlEntry
        {
            LongURL = model.LongURL,
            ShortURL = urlHash,
            CreationDate = DateTime.Now,
            Clicks = 0
        };
        await _context.UrlEntries.AddAsync(newEntry);

        await _context.SaveChangesAsync();

        await _context.UserLinks.AddAsync(new UserLink
        {
            UserId = CurrentUserId,
            UrlId = newEntry.Id
        });


        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteAsync(int id)
    {
        var urlEntry = await _context.UrlEntries.FirstOrDefaultAsync(u => u.Id == id);
        if (urlEntry != null)
        {
            _context.UrlEntries.Remove(urlEntry);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    private async Task<IEnumerable<UrlEntry>> GetUrlsCurrentUserAsync()
    {
        
        var urls = await _context.UserLinks
            .Where(u => u.UserId == CurrentUserId)
            .ToListAsync();
        return await _context.UrlEntries
            .Where(e => urls.Select(u => u.UrlId).Contains(e.Id))
            .ToListAsync();
    }


    /*
    public IActionResult Index()
    {
        return View();
    }
    */
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string GenerateUrlHash(string longUrl, int length = 9)
    {
        //var selfUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(longUrl));

            string urlHash = Convert.ToBase64String(hashBytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "")
                .Substring(0, length);

            // ���������� ������ URL � �����
            return urlHash;
        }
    }

    private string GetCurrentUrl()
    {
        var request = HttpContext.Request;
        var host = request.Host.ToUriComponent(); // �������� ���� � ����
        var scheme = request.Scheme; // �������� ����� (http ��� https)
        var currentUrl = $"{scheme}://{host}/";

        return currentUrl;
    }
    //[Authorize]
    //[HttpGet("/{shortUrl}")]
    //[HttpGet("/{shortUrl:regex(^[[a-zA-Z0-9]]+$)}")]
    /*
    public async Task<IActionResult> RedirectShortUrl(string shortUrl)
    {
        var entry = await _context.UrlEntries.FirstOrDefaultAsync(e => e.ShortURL == shortUrl);
        if (entry == null)
        {
            //����� ������ �� ����������. �������� �� �������.
            return NotFound();
        }
        entry.Clicks++;
        _context.Update(entry);
        await _context.SaveChangesAsync();

        return Redirect(entry.LongURL);
    }
    */
}
