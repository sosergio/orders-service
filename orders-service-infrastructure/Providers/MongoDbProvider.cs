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
    public class MongoDbProvider : IDbProvider
    {
        DbConfig _dbConfig;
        public MongoDbProvider(DbConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public Task<IDbEntity> CreateDocumentAsync(string collectionName, IDbEntity document)
        {
            throw new NotImplementedException();
        }

        public Task<IDbEntity> ReadDocumentAsync(string collectionName, string documentId)
        {
            throw new NotImplementedException();
        }

        public Task<IDbEntity> ReplaceDocumentAsync(string collectionName, IDbEntity document)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Search<T>(string collectionName)
        {
            throw new NotImplementedException();
        }
    }
}
