using CIS325_Master_Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CIS325_Master_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Registers EF Core with SQLite connection
            builder.Services.AddDbContext<AdvisingContext>(opts =>
                opts.UseSqlite(builder.Configuration.GetConnectionString("Advising")));
            //In there, we are using a local SQLite database. In production, we need to
            //switch to a SQL enterprise DBMS (or other enterprise class DBMS).


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
