
using Microsoft.EntityFrameworkCore;
using FICCI_API.ModelsEF;
using FICCI_API.Models;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FICCI_DB_APPLICATIONSContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FICCI");
    }
    );
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Swagger}/{action=Index}");
app.UseAuthorization();



app.Run();
