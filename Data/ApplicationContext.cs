using Angular_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Angular_2.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {            
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

    }
}
