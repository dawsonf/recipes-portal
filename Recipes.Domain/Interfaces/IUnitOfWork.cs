using System;

namespace Recipes.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRecipeRepository Recipes { get; }
        int Complete();
    }
}
