using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipes.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task <int> ExecWithStoredProcedure(string query, params object[] parameters);
        Task<List<T>> ExecWithStoredProcedureResults(string query, params object[] parameters);
        Task<List<T>> ExecWithStoredProcedureResults(string query);

    }
}
