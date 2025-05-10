using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using SqlServerCache;

var builder = WebApplication.CreateBuilder(args);

// Configure Redis connection
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration["SqlServerCache:ConnectionString"];
    options.SchemaName = "dbo";
    options.TableName = "Cache";
});

// Register the custom CacheService
builder.Services.AddScoped<CacheService>();

var app = builder.Build();

app.MapGet("/", async context =>
{
    var cacheService = context.RequestServices.GetRequiredService<CacheService>();

    string cacheKey = "ProductList";
    string? cachedData = await cacheService.GetSqlCacheAsync(cacheKey);

    if (!string.IsNullOrEmpty(cachedData))
    {
        Console.WriteLine("Cache Hit: " + cachedData);
        await context.Response.WriteAsync($"Cache Hit: {cachedData}");
    }
    else
    {
        Console.WriteLine("Cache Miss: Generating new data...");
        string productData = "Product1, Product2, Product3";

        await cacheService.SetSqlCacheAsync(cacheKey, productData, TimeSpan.FromMinutes(5));
        Console.WriteLine("Data Stored in Cache: " + productData);
        await context.Response.WriteAsync($"Cache Miss: {productData} - Stored in Cache");
    }
});

await app.RunAsync();