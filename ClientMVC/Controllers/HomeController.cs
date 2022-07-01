﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using ClientMvc.Models;
using Newtonsoft.Json;
using ClientMvc.ModelViews;

namespace ClientMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return SignOut("Cookie", "oidc");
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            /*            var idToken = await HttpContext.GetTokenAsync("id_token");

                        var jwtAccessTokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                        var jwtIdTokenHandler = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

                        var claims = User.Claims.ToList();*/


            var result = await GetCalendarInfo(accessToken);

            EventsViewModel eventsViewModel = new EventsViewModel();
            eventsViewModel.AllEvents = (List<EventDto>)result;

            return View(nameof(Secret), eventsViewModel);
        }

        private async Task<IEnumerable<EventDto>> GetCalendarInfo(string accessToken)
        {
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetStringAsync("https://localhost:5003/api/Calendar");

            await RefreshAccessToken();

            return JsonConvert.DeserializeObject<List<EventDto>>(response);
        }

        private async Task RefreshAccessToken()
        {
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44305/");

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshTokenClient = _httpClientFactory.CreateClient();

            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    RefreshToken = refreshToken,
                    ClientId = "client_id_mvc",
                    ClientSecret = "client_secret_mvc"
                });

            var authInfo = await HttpContext.AuthenticateAsync("Cookie");

            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);
        }
    }
}