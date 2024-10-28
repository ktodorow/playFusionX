using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using playFusionX.Data.Models;
using playFusionX.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace playFusionX.Controllers
{
    public class SpotifyController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _dbContext;

        public SpotifyController(IConfiguration config, ApplicationDbContext dbContext)
        {
            _config = config;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Connect()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var spotifyToken = await _dbContext.SpotifyTokens.FirstOrDefaultAsync(t => t.UserId == userId);

            if (spotifyToken != null)
            {
                // User is already authorized, redirect them to profile page
                return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
            }

            var clientId = _config["Spotify:ClientId"];
            var redirectUri = _config["Spotify:RedirectUri"];
            var scope = "user-read-private user-library-read playlist-modify-public";

            var authorizationUrl = $"https://accounts.spotify.com/authorize?client_id={clientId}&response_type=code&redirect_uri={redirectUri}&scope={scope}";

            return Redirect(authorizationUrl);
        }
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code not provided");
            }

            using var client = new HttpClient();
            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", _config["Spotify:RedirectUri"] },
                { "client_id", _config["Spotify:ClientId"] },
                { "client_secret", _config["Spotify:ClientSecret"] }
            };

            var content = new FormUrlEncodedContent(requestBody);
            var response = await client.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<SpotifyTokenReponse>(responseContent);

                // Save tokens in the database
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var spotifyToken = new SpotifyToken
                {
                    UserId = userId,
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken,
                    TokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
                };

                _dbContext.SpotifyTokens.Add(spotifyToken);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return BadRequest("Error during Spotify authentication");
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            using var client = new HttpClient();
            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", _config["Spotify:ClientId"] },
                { "client_secret", _config["Spotify:ClientSecret"] }
            };

            var content = new FormUrlEncodedContent(requestBody);
            var response = await client.PostAsync("https://accounts.spotify.com/api/token", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);
                return tokenResponse.access_token;
            }

            return null;
        }
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var spotifyToken = await _dbContext.SpotifyTokens.FirstOrDefaultAsync(t => t.UserId == userId);

            if (spotifyToken != null)
            {
                _dbContext.SpotifyTokens.Remove(spotifyToken);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        // public async Task<IActionResult> CheckPremiumStatus()
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     var spotifyToken = await _dbContext.SpotifyTokens.FirstOrDefaultAsync(t => t.UserId == userId);

        //     if (DateTime.UtcNow > spotifyToken.TokenExpiry)
        //     {
        //         // Refresh the access token
        //         var newAccessToken = await RefreshAccessToken(spotifyToken.RefreshToken);
        //         if (!string.IsNullOrEmpty(newAccessToken))
        //         {
        //             spotifyToken.AccessToken = newAccessToken;
        //             spotifyToken.TokenExpiry = DateTime.UtcNow.AddSeconds(3600); // Set expiry for one hour
        //             await _dbContext.SaveChangesAsync();
        //         }
        //     }

        //     using var client = new HttpClient();
        //     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", spotifyToken.AccessToken);
        //     var response = await client.GetAsync("https://api.spotify.com/v1/me");

        //     if (response.IsSuccessStatusCode)
        //     {
        //         var content = await response.Content.ReadAsStringAsync();
        //         dynamic profile = JsonConvert.DeserializeObject(content);
        //         string product = profile.product.ToString().ToLower();

        //         bool isPremium = product == "premium" || product == "duo" || product == "family";
        //         ViewData["IsSpotifyPremium"] = isPremium;
        //     }
        //     else
        //     {
        //         ViewData["IsSpotifyPremium"] = false;
        //     }

        //     return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        // }
    }
}