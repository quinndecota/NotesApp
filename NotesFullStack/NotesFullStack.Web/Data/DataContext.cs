using Microsoft.EntityFrameworkCore;

using NotesFullStack.Web.Data.Entities;

namespace NotesFullStack.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
