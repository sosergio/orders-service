using OrdersService.Core.DataModels;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService.Core.Interfaces
{
    public interface IDbProvider
    {
        IQueryable<T> Search<T>(string collectionName);
        Task<IDbEntity> ReplaceDocumentAsync(string collectionName, IDbEntity document);
        Task<IDbEntity> ReadDocumentAsync(string collectionName, string documentId);
        Task<IDbEntity> CreateDocumentAsync(string collectionName, IDbEntity document);
    }
}
