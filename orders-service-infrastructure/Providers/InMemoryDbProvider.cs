using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OrdersService.Core.Config;
using OrdersService.Core.DataModels;
using OrdersService.Core.Interfaces;

namespace OrdersService.Infrastructure.Providers
{
    public class InMemoryDbProvider : IDbProvider
    {
        private static Dictionary<string, List<IDbEntity>> _db { get; set; } = new Dictionary<string, List<IDbEntity>>();

        public InMemoryDbProvider()
        {}

        public void CreateCollectionIfNotExists(string collectionId)
        {
            if (!_db.Keys.Contains(collectionId))
            {
                _db.Add(collectionId, new List<IDbEntity>());
            }
        }

        public Task<IDbEntity> CreateDocumentAsync(string collectionName, IDbEntity document)
        {
            CreateCollectionIfNotExists(collectionName);
            document.Id = Guid.NewGuid().ToString();
            _db[collectionName].Add(document);
            return Task.FromResult(document);
        }

        public Task<IDbEntity> ReadDocumentAsync(string collectionName, string documentId)
        {
            CreateCollectionIfNotExists(collectionName);
            return Task.FromResult(_db[collectionName].SingleOrDefault(d => d.Id == documentId));
        }

        public Task<IDbEntity> ReplaceDocumentAsync(string collectionName, IDbEntity document)
        {
            CreateCollectionIfNotExists(collectionName);
            _db[collectionName] = _db[collectionName].Where(d => d.Id != document.Id).ToList();
            _db[collectionName].Add(document);
            return Task.FromResult(document);
        }

        public IQueryable<T> Search<T>(string collectionName)
        {
            CreateCollectionIfNotExists(collectionName);
            return _db[collectionName].Select(x => (T)x).AsQueryable<T>();
        }
    }
}
