using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Models;

namespace Recipes.Repository
{
   public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        //No migrations - DB First approach
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

    }
}
