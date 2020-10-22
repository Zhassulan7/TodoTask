using Microsoft.EntityFrameworkCore;

namespace TodoTask.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Task> Tasks { get; set; }

    }
}
