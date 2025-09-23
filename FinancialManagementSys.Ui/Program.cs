using FinancialManagement.Application;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Infrastructure;
using FinancialManagement.Infrastructure.Data;
using FinancialManagement.Web.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddHttpClient<IApiService, ApiService>((sp, client) =>
{
    var config = builder.Configuration.GetSection("ApiSettings");
    var baseUrl = config["BaseUrl"] ?? "https://localhost:7162/api/";
    if (!baseUrl.EndsWith("/")) baseUrl += "/";
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("User-Agent", "FinancialManagement-WebApp");
    client.Timeout = TimeSpan.FromSeconds(int.Parse(config["Timeout"] ?? "30"));
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});


builder.Services.AddHttpContextAccessor();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "FinancialManagement.Session";
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Add Memory Cache
builder.Services.AddMemoryCache();

// Cookie Authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.Cookie.Name = "FinancialManagement.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

// Register repositories
builder.Services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();

// Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "FinancialManagement.Antiforgery";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApi", policy =>
    {
        policy.WithOrigins("https://localhost:5001", "https://localhost:7162")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowApi");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "auth",
    pattern: "auth/{action=Login}",
    defaults: new { controller = "Auth" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "dashboard",
    pattern: "dashboard",
    defaults: new { controller = "Dashboard", action = "Index" });

app.MapControllerRoute(
    name: "accounts",
    pattern: "accounts/{action=Index}/{id?}",
    defaults: new { controller = "ChartOfAccounts" });

app.MapControllerRoute(
    name: "journal",
    pattern: "journal/{action=Index}/{id?}",
    defaults: new { controller = "JournalEntry" });


app.MapGet("/api/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
}));

app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString().ToLower();
    var isAuthPath = path.StartsWith("/auth");
    var isStaticFile = path.StartsWith("/css") || path.StartsWith("/js") ||
                       path.StartsWith("/lib") || path.StartsWith("/images") ||
                       path.StartsWith("/favicon");
    var isHealthCheck = path.StartsWith("/api/health");

    if (!isAuthPath && !isStaticFile && !isHealthCheck)
    {
        var token = context.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
        {
            context.Response.Redirect("/auth/login");
            return;
        }
    }

    await next();
});

try
{
    app.Run();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Application startup failed");
    throw;
}
