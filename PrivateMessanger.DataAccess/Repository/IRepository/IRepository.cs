using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PrivateMessanger.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool >> filter, string? includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);

        Task AddAsync(T entity);
        /*  
            Why there is no implementation of Update and Save methods here:
            When you update one model logic might be different from updating another model,
            thus no implementation here. But when making, for example, CategoryRepository,
            there will be an implemantation of Update and Save methods 
        */
        // void Update(T entity);
        // void Save();
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);

    }
}
