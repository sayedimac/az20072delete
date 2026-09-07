using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheChatApp.Models;

namespace TheChatApp.Controllers;

/// <summary>
/// Handles requests for the application's main pages.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Displays the home page.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays the privacy page.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Displays error details without allowing the response to be cached.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // Use the current activity ID when available; otherwise, use the HTTP trace ID.
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
