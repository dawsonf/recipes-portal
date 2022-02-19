using Recipes.Domain.Interfaces;
using System;


namespace Recipes.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository Users { get; }
        public IRecipeRepository Recipes { get; }

        public UnitOfWork(ApplicationDbContext recipeDbContext,
            IUserRepository userRepository,
            IRecipeRepository recipeRepository)
        {
            this._context = recipeDbContext;

            this.Users = userRepository;
            this.Recipes = recipeRepository;
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
