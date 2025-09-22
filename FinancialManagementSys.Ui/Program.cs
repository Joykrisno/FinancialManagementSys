using FinancialManagement.Application;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Infrastructure;
using FinancialManagement.Infrastructure.Data;
using FinancialManagement.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add HttpClient for API calls
builder.Services.AddHttpClient("ApiClient", client =>
{
    // Read BaseUrl from configuration
    var baseUrl = builder.Configuration.GetSection("ApiSettings:BaseUrl").Value;

    // If not configured, fallback to your API swagger base URL
    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        baseUrl = "https://localhost:7162/"; // আপনার API URL
    }

    // Ensure trailing slash for proper URI combination
    if (!baseUrl.EndsWith("/")) baseUrl += "/";

    client.BaseAddress = new Uri(baseUrl);

    // Optional timeout configuration
    client.Timeout = TimeSpan.FromSeconds(
        builder.Configuration.GetValue<int>("ApiSettings:Timeout", 30)
    );
});

// Add JWT Authentication for Web App
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "YourSuperSecretKeyHere123456789012345678901234567890");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Add session support for JWT token storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register ApiService
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
// Register IHttpContextAccessor for DI
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
