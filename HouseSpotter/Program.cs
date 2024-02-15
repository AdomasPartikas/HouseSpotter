using HouseSpotter.Scrapers;
using HouseSpotter.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<ScraperClient>();
builder.Services.AddSingleton<InMemoryLoggerProvider>();
builder.Services.AddScoped<ScraperForAruodas>();

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddProvider(builder.Services.BuildServiceProvider().GetRequiredService<InMemoryLoggerProvider>());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();


