﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            //retrieve access token
            var serverClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44305/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    //AllowedScopes = { "WeatherApi", "CalendarApi"},
                    Scope = "WeatherApi"
                });
        
            //retrieve secret data
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:5007/api/Weather?lat=23&lon=23&cnt=3");

            var content = await response.Content.ReadAsStringAsync();



            var tokenResponseCalendar = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    Scope = "CalendarApi",
                });
            //retrieve secret data
            apiClient.SetBearerToken(tokenResponseCalendar.AccessToken);

            var responseCalendar = await apiClient.GetAsync("https://localhost:5003/api/Calendar");

            var contentCalendar = await responseCalendar.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content 
                + tokenResponseCalendar.AccessToken
                + contentCalendar,
            });
        }     
    }
}
