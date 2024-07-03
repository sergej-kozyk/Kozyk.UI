using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Srv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Kozyk.API.Data;

namespace Kozyk.API.Data
{

    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {

            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //Выполнение миграций;
            await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.Serving.Any())
            {
                var categories = new Category[]
            {
            new Category {GroupName="Столовый",
            NormalizedName="Сервировка"},
            new Category {GroupName="Кухонный",
            NormalizedName="Готовка"},
            };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();


               var _serving = new List<Serving>
        {
           new Serving {Name="Столовый набор 01",
            Description="Набор для сервировки",
            Image= uri + "Images/010.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Сервировка"))},

            new Serving {Name="Столовый набор 02",
            Description="Набор для приготовлении пищи",
            Image= uri + "Images/020.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Готовка"))},

            new Serving {Name="Столовый набор 03",
            Description="Набор для сервировки",
            Image=uri + "Images/030.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Сервировка"))},

             new Serving {Name="Столовый набор 04",
            Description="Набор для сервировки",
            Image=uri + "Images/040.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Готовка"))},

              new Serving {Name="Столовый набор 05",
            Description="Набор для приготовлении пищи",
            Image=uri + "Images/050.png",
            Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("Сервировка"))},

            };
                await context.Serving.AddRangeAsync(_serving);
                await context.SaveChangesAsync();

            }
        }
    }
}

