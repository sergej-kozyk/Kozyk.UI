using Srv.Domain.Entities;
using Srv.Domain.Models;

namespace Kozyk.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
            new Category {Id=1, GroupName="Столовый",
            NormalizedName="Сервировка"},
            new Category {Id=2, GroupName="Кухонный",
            NormalizedName="Готовка"},
            };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }

       
    }
}
