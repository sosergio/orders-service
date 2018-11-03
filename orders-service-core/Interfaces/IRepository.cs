using OrdersService.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Core.Interfaces
{
    public interface IRepository<T> where T : IDbEntity
    {
        Task<T> SaveAsync(T item);
        void Delete(string itemId);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate);
        Task<T> LoadById(string itemId);
    }
}
