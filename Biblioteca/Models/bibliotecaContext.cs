using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace Biblioteca.Models
{
    public class bibliotecaContext : DbContext
    {
        public bibliotecaContext(DbContextOptions<bibliotecaContext> options) : base(options)
        {
        }

        public DbSet<Autor> autor { get; set; }
        public DbSet<Libro> libro { get; set; }
    }
}





