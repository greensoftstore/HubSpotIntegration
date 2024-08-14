var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient and configure it for HubSpotService
builder.Services.AddHttpClient<HubSpotService>(client =>
{
    client.BaseAddress = new Uri("https://api.hubapi.com/");
});

// Add HubSpotService with the API key from configuration
builder.Services.AddSingleton(provider => new HubSpotService(
    provider.GetRequiredService<HttpClient>(),
    builder.Configuration["HubSpot:ApiKey"] // Assuming you store the API key in appsettings.json
));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HubSpot}/{action=index}/{id?}");

app.Run();
