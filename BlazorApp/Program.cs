using BlazorApp;
using BlazorApp.Data;
using BlazorApp.Store;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<ChoreService>();
builder.Services.AddScoped<StreakChallengeService>();
builder.Services.AddScoped<SingleChallengeService>();
builder.Services.AddScoped<AccountsService>();
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseInMemoryDatabase("MyDatabase"));

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();