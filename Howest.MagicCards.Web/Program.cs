using Howest.MagicCards.Web.Pages;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<Cards>();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddDbContext<MyCardContext>
    (options => options.UseSqlServer(config.GetConnectionString("mtg_v1")));
builder.Services.AddAutoMapper(new Type[] { typeof(CardsProfile) });

builder.Services.AddHttpClient("CardsAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:44334/api/v1.1/");
});

builder.Services.AddHttpClient("CardsAPIv1.5", client =>
{
    client.BaseAddress = new Uri("https://localhost:44334/api/v1.5/");
});

builder.Services.AddHttpClient("DeckMinimalAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:44320/api/");
});

WebApplication app = builder.Build();

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
