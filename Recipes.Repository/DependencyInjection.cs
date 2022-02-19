using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Recipes.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRecipeRepository, RecipeRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<ApplicationDbContext>(opt => opt
                    .UseSqlServer("Server=desktop-1qv22c2;Database=RecipesDB;Integrated Security=True;"));
            return services;
        }
    }
}
