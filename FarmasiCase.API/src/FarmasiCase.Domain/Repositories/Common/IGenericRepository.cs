using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Domain.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(string entityId);
        Task<TEntity> CreateAsync(TEntity entity);
        Task UpdateAsync(string entityId, TEntity entity);
        Task DeleteAsync(string entityId);
    }
}
