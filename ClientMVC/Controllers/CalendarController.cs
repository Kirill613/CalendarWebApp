using AutoMapper;
using ClientMvc.Models;
using ClientMvc.Controllers;
using ClientMvc.ModelViews;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherAPI.Services.Models;

namespace ClientMvc.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public CalendarController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        // GET: CalendarController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CalendarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CalendarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EvCreateViewModel evCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", evCreateViewModel) });
            }

            if (IsBeginLessThanEndAndIsDayTheSame(evCreateViewModel.BeginTime, evCreateViewModel.EndTime))
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", evCreateViewModel) });
            }

            try
            {
                var eventDto = _mapper.Map<EventDto>(evCreateViewModel);
                var res = await CreateEvent(eventDto);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Secret", await GetSecretModel()) });
            }
            catch
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", evCreateViewModel) });
            }
        }

        // POST: CalendarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var res = await DeleteEvent(id);
                return RedirectToAction("Secret");
            }
            catch
            {
                return RedirectToAction("Secret");
            }
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await GetEventById(id);

            var evCreateViewModel = _mapper.Map<EvCreateViewModel>(response);

            return View(evCreateViewModel);
        }

        // POST: Chapters/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(EvCreateViewModel evCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", evCreateViewModel) });
            }

            if (IsBeginLessThanEndAndIsDayTheSame(evCreateViewModel.BeginTime, evCreateViewModel.EndTime))
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", evCreateViewModel) });
            }

            try
            {
                var eventDto = _mapper.Map<EventDto>(evCreateViewModel);
                var res = await EditEvent(eventDto);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Secret", await GetSecretModel()) });
                //return RedirectToAction("Secret", "Home");
            }
            catch
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", evCreateViewModel) });
            }
        }
        public async Task<IActionResult> Secret()
        {
            return View(nameof(Secret), await GetSecretModel());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUI(DateTime date)
        {
            EventsViewModel vm = new EventsViewModel();
            vm = await GetSecretModel();
            vm.date = date;
            return View(nameof(Secret), vm);
        }


        private async Task<EventsViewModel> GetSecretModel()
        {
            var result = await GetCalendarInfo();

            EventsViewModel eventsViewModel = new EventsViewModel();
            eventsViewModel.AllEvents = (List<EventDto>)result;

            var resTemperatures = await GetWeather();

            var beginForecastTime = DateOnly.FromDateTime(DateTime.Now);
            var endForecastTime = beginForecastTime.AddDays(4);

            Dictionary<DateOnly, List<EventDto>> events = new Dictionary<DateOnly, List<EventDto>>();
            Dictionary<EventDto, int> eventsWithWeather = new Dictionary<EventDto, int>();

            foreach (EventDto eventDto in eventsViewModel.AllEvents)
            {
                var currDay = DateOnly.FromDateTime(eventDto.BeginTime);
                if (!events.ContainsKey(currDay))
                {
                    events.Add(currDay, new List<EventDto>() { eventDto });
                }
                else
                {
                    List<EventDto> currDayList = events[currDay];
                    currDayList.Add(eventDto);
                    events[currDay] = currDayList;
                }             

                if (currDay >= beginForecastTime && currDay <= endForecastTime)
                {
                    eventsWithWeather.Add(eventDto, FindClosestTemp(resTemperatures, eventDto));
                }
            }

            eventsViewModel.AllElementsByDays = events;
            eventsViewModel.AllForecast5Days = eventsWithWeather;
           
            return eventsViewModel;
        }
        private int FindClosestTemp(WeatherDataDto resTemperatures, EventDto eventDto)
        {
            Int32 unixEndTime = (int)eventDto.EndTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Int32 unixBeginTime = (int)eventDto.BeginTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            int timeMiddle = (unixEndTime / 2 + unixBeginTime / 2) ;
        
            int min = 0;
            WeatherListDto dtMinTime = new WeatherListDto();
            foreach(var item in resTemperatures.list)
            {
                int result = Math.Abs(timeMiddle - item.dt);

                if (result <= min || min == 0)
                {
                    min = result;
                    dtMinTime = item;
                }
            }

            return (int)(dtMinTime.main.temp - 273.15);
        }
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        private bool IsBeginLessThanEndAndIsDayTheSame(DateTime beginTime, DateTime endTime)
        {
            if (beginTime.Date != endTime.Date)
            {
                return true;
            }

            if (beginTime.Hour > endTime.Hour)
            {
                return true;
            }

            if ((beginTime.Hour == endTime.Hour) && (beginTime.Minute >= endTime.Minute))
            {
                return true;
            }

            return false;
        }
        private async Task<WeatherDataDto> GetWeather()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetStringAsync("https://localhost:5007/api/Weather");

            await RefreshAccessToken();

            return JsonConvert.DeserializeObject<WeatherDataDto>(response);
        }
        private async Task<IEnumerable<EventDto>> GetCalendarInfo()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetStringAsync("https://localhost:5003/api/Calendar");

            await RefreshAccessToken();

            return JsonConvert.DeserializeObject<List<EventDto>>(response);
        }
        private async Task<IActionResult> CreateEvent(EventDto eventDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.PostAsJsonAsync<EventDto>("https://localhost:5003/api/Calendar", eventDto);

            await RefreshAccessToken();

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<IActionResult> DeleteEvent(Guid id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.DeleteAsync($"https://localhost:5003/api/Calendar/{id}");

            await RefreshAccessToken();

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<IActionResult> EditEvent(EventDto eventDto)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.PutAsJsonAsync($"https://localhost:5003/api/Calendar", eventDto);

            await RefreshAccessToken();

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<EventDto> GetEventById(Guid id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetStringAsync($"https://localhost:5003/api/Calendar/{id}");

            await RefreshAccessToken();

            return JsonConvert.DeserializeObject<EventDto>(response);
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
