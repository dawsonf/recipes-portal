using Recipes.Repository;
using Recipes.Domain.Interfaces;
using Recipes.Domain.Models;

namespace Recipes.Repository
{
    class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
