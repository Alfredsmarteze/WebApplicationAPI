using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Context
{
    public class ModelContext:DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options) { }

        public DbSet<Users> users { get; set; }
        public DbSet<UserLogins> userLogins { get; set; }
        public DbSet<UserTokens> userTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var conString = config.GetConnectionString("Connection");
            optionsBuilder.UseSqlServer(conString);
        }
    }
}
