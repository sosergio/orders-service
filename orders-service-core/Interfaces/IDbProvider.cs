using OrdersService.Core.DataModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrdersService.Core.Interfaces
{
    public interface IDbProvider

    {
        Task<IQueryable<T>> Search<T>(string collectionName, Expression<Func<T, bool>> predicate);
        Task<IDbEntity> ReplaceDocumentAsync(string collectionName, IDbEntity document);
        Task<IDbEntity> ReadDocumentAsync(string collectionName, string documentId);
        Task<IDbEntity> CreateDocumentAsync(string collectionName, IDbEntity document);
    }
}
