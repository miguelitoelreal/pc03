using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Models;
using NewsPortal.Services;

namespace NewsPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PostService _postService;

    public HomeController(ILogger<HomeController> logger, PostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _postService.GetEnrichedPostsAsync();
        return View(posts);
    }

    public async Task<IActionResult> Post(int id)
    {
        var post = await _postService.GetEnrichedPostByIdAsync(id);
        if (post == null) return NotFound();
        return View(post);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
