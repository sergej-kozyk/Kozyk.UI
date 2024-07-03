using Microsoft.EntityFrameworkCore;
using Srv.Domain.Entities;


namespace Kozyk.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Serving> Serving { get; set; }
        public DbSet<Category> Categories { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



    }
}
