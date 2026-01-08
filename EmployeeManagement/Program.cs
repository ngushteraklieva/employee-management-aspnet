using EmployeeManagement.Data;
using EmployeeManagement.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //In Entity Framework Core, the DB context (AppDbContext in your code) is a central class
            //that acts as a bridge between your application and the database. 
            //The value of a DbContext instance represents a single session with the database.
            //It is not a static piece of data; it is a live connection used to query and save data.
            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseInMemoryDatabase("EmployeeDb")
                );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyCors", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            //add the employee repo to the DI in the life time of scoped ->
            //every time when we have a http request we'll have a brand new instance of the employee repo
            //1st we specify the interface(IEmploye..) and then the actual class -> type safety
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V!");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("MyCors");

            app.MapControllers();

            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
