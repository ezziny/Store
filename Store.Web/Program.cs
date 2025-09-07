using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using Store.Data.Contexts;
using Store.Repository;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Services.HandleResponses;
using Store.Services.Services.Product;
using Store.Services.Services.Product.Dtos;
using Store.Web.Extensions;
using Store.Web.Helper;
using Store.Web.Middlewares;

namespace Store.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        }
        );
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
        });
        builder.Services.AddAplicationServices();

        var app = builder.Build();
        await ApplySeeding.ApplySeedingAsync(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
