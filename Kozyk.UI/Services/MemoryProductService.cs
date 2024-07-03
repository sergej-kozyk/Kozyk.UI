

using Microsoft.AspNetCore.Mvc;
using Srv.Domain.Entities;
using Srv.Domain.Models;
using static Azure.Core.HttpHeader;

namespace Kozyk.UI.Services
{
    
        public class MemoryProductService: IProductService
        {
            List<Serving> _serving;
            List<Category> _categories;
            IConfiguration _config;



            public MemoryProductService(ICategoryService categoryService, [FromServices] IConfiguration config)
            {
                _config = config;
                _categories = categoryService.GetCategoryListAsync()
                    .Result
                    .Data;

                SetupData();
            }



        /// <summary>
        /// Инициализация списков
        /// </summary>
        public void SetupData()
        {

            _serving = new List<Serving>
        {
            new Serving {Id = 1, Name="Столовый набор 01",
            Description="Набор для сервировки",
            Image="Images/01.png",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Сервировка")).Id},

            new Serving {Id = 2, Name="Столовый набор 02",
            Description="Набор для приготовлении пищи",
            Image="Images/02.png",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Готовка")).Id },

            new Serving {Id = 3, Name="Столовый набор 03",
            Description="Набор для сервировки",
            Image="Images/03.png",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Сервировка")).Id },

             new Serving {Id = 4, Name="Столовый набор 04",
            Description="Набор для сервировки",
            Image="Images/04.png",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Готовка")).Id },

              new Serving {Id = 5, Name="Столовый набор 05",
            Description="Набор для приготовлении пищи",
            Image="Images/05.png",
            CategoryId = _categories.Find(c=>c.NormalizedName.Equals("Сервировка")).Id },

            };

        }
        Task<ResponseData<ListModel<Serving>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo=1)
        {


                // Создать объект результата
                var result = new ResponseData<ListModel<Serving>>();

                // Id категории для фильрации
                int? categoryId = null;

                // если требуется фильтрация, то найти Id категории
                // с заданным categoryNormalizedName
                if (categoryNormalizedName != null)
                    categoryId = _categories
                    .Find(c =>
                    c.NormalizedName.Equals(categoryNormalizedName))
                    ?.Id;

                // Выбрать объекты, отфильтрованные по Id категории,
                // если этот Id имеется
                var data = _serving
                .Where(d => categoryId == null || d.CategoryId.Equals(categoryId))?
                .ToList();

                // получить размер страницы из конфигурации
                int pageSize = _config.GetSection("ItemsPerPage").Get<int>();


                // получить общее количество страниц
                int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

                // получить данные страницы
                var listData = new ListModel<Serving>()
                {
                    Items = data.Skip((pageNo - 1) *
                pageSize).Take(pageSize).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                };

                // поместить ранные в объект результата
                result.Data = listData;



                // Если список пустой
                if (data.Count == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Нет объектов в выбраннной категории";
                }
                // Вернуть результат
                return Task.FromResult(result);

            }



        public Task<ResponseData<Serving>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Serving product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Serving>> CreateProductAsync(Serving product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }


    }
    }
