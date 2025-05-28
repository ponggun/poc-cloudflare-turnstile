using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TurnstilePoC.Models;

namespace TurnstilePoC.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewData["TurnstileSiteKey"] = _configuration["Turnstile:SiteKey"];
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Get the token from form
        string token = Request.Form["cf-turnstile-response"];
        if (string.IsNullOrEmpty(token))
        {
            ModelState.AddModelError(string.Empty, "Verification failed. Please try again.");
            return View(model);
        }

        // Get the remote IP
        string remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

        // Prepare verification data
        var formData = new Dictionary<string, string>
        {
            { "secret", _configuration["Turnstile:SecretKey"] ?? Environment.GetEnvironmentVariable("TURNSTILE_SECRET") ?? "" },
            { "response", token },
            { "remoteip", remoteIp }
        };

        try
        {
            var response = await _httpClient.PostAsync(
                "https://challenges.cloudflare.com/turnstile/v0/siteverify", 
                new FormUrlEncodedContent(formData));
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var verifyResponse = JsonSerializer.Deserialize<VerifyResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Log the full verification response
            _logger.LogInformation("Turnstile verification response: {Response}", responseContent);

            if (verifyResponse?.Success == true)
            {
                // In a real application, you would authenticate the user here
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Verification failed. Please try again.");
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying Turnstile token");
            ModelState.AddModelError(string.Empty, "An error occurred during verification. Please try again.");
            return View(model);
        }
    }
}