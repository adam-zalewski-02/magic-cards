using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddApiVersioning(o =>
{
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});


builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";

        options.SubstituteApiVersionInUrl = true;
    }

);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "Magic The Gathering API version 1.1",
        Version = "v1.1",
        Description = "API to manage MTG cards"
    });
    c.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "Magic The Gathering API version 1.5",
        Version = "v1.5",
        Description = "API to manage MTG cards"
    });
});

builder.Services.AddDbContext<MyCardContext>
    (options => options.UseSqlServer(config.GetConnectionString("mtg_v1")));
builder.Services.AddScoped<ICardRepository, SqlCardRepository>();
builder.Services.AddScoped<IArtistRepository, SqlArtistRepository>();
builder.Services.AddScoped<IRarityRepository, SqlRarityRepository>();
builder.Services.AddScoped<ISetRepository, SqlSetRepository>();
builder.Services.AddScoped<IColorRepository, SqlColorRepository>();

builder.Services.AddAutoMapper(new Type[]
{
    typeof(CardsProfile),
    typeof(ArtistsProfile),
    typeof(RarityProfile),
    typeof(SetProfile),
    typeof(ColorsProfile)
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MTG API v1.1");
        c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "MTG API v1.5");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
