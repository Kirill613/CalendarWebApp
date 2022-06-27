var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(config => {
    config.DefaultScheme = "Cookie";
    config.DefaultChallengeScheme = "oidc";
})
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config => {
                    config.Authority = "https://localhost:44305/";
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;

                    config.ResponseType = "code";

                    config.SignedOutCallbackPath = "/Home/Index";

                    config.GetClaimsFromUserInfoEndpoint = true;

                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    //config.Scope.Add("profile");
                    config.Scope.Add("WeatherApi");
                    config.Scope.Add("CalendarApi");
                    config.Scope.Add("offline_access");
                });

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
