using Kozyk.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Srv.Domain.Entities;
using Srv.Domain.Models;

namespace Kozyk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServingController(AppDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;

        }


        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Serving>>>> GetProductListAsync(
              string? category,
              int pageNo = 1,
              int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Serving>>();

            // Фильтрация по категории загрузка данных категории
            var data = _context.Serving
            .Include(d => d.Category)
            .Where(d => String.IsNullOrEmpty(category)
            || d.Category.NormalizedName.Equals(category));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
            if (pageNo > totalPages)
                pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<Serving>()
            {
                Items = await data
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // поместить данные в объект результата
            result.Data = listData;
            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }
        // GET: api/
        [HttpGet("{id}")]
        public async Task<ActionResult<Serving>> GetServing(int id)
        {
            var serving = await _context.Serving.FindAsync(id);

            if (serving == null)
            {
                return NotFound();
            }

            return serving;
        }

        // PUT: api/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServing(int id, Serving serving)
        {
            if (id != serving.Id)
            {
                return BadRequest();
            }

            _context.Entry(serving).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Serving>> PostServing(Serving serving)
        {
            _context.Serving.Add(serving);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServing", new { id = serving.Id }, serving);
        }

        // DELETE: api/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServing(int id)
        {
            var serving = await _context.Serving.FindAsync(id);
            if (serving == null)
            {
                return NotFound();
            }

            _context.Serving.Remove(serving);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServingExists(int id)
        {
            return _context.Serving.Any(e => e.Id == id);
        }

        [HttpPost("{id}")]

        public async Task<IActionResult> SaveImage(int id, IFormFile image, [FromServices] IWebHostEnvironment env)
        {
            // Найти объект по Id
            var serving = await _context.Serving.FindAsync(id);
            if (serving == null)
            {
                return NotFound();
            }

            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(env.WebRootPath, "Images");

            // получить случайное имя файла
            var randomName = Path.GetRandomFileName();

            // получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);

            // задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);

            // полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);

            // создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);

            // скопировать файл в поток
            await image.CopyToAsync(stream);

            // получить Url хоста
            var host = "https://" + Request.Host;

            // Url файла изображения
            var url = $"{host}/Images/{fileName}";

            // Сохранить url файла в объекте
            serving.Image = url;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
