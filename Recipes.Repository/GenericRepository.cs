using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using Recipes.Domain.Models;
using System.Linq;

namespace Recipes.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        protected GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Stored Procedures
        async Task<int> IGenericRepository<T>.ExecWithStoredProcedure(string query, params object[] parameters)
        {
            // Returns rows affected
            return await _context.Database.ExecuteSqlRawAsync(query, parameters);
        }

        async Task<List<T>> IGenericRepository<T>.ExecWithStoredProcedureResults(string query, params object[] parameters)
        {
            //Returns dataset
            var result = await _context.Set<T>().FromSqlRaw(query, parameters).ToListAsync();
            return result;
        }

        async Task<List<T>> IGenericRepository<T>.ExecWithStoredProcedureResults(string query)
        {
            //Returns dataset
            var result = await _context.Set<T>().FromSqlRaw(query).ToListAsync();
            return result;
        }

        #endregion

        #region CRUD
        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        #endregion

    }
}
