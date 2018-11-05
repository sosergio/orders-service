using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrdersService.Core.Config;
using OrdersService.Core.DataModels;
using OrdersService.Core.Interfaces;

namespace OrdersService.Infrastructure.Providers
{
    public class MongoDbProvider : BaseMongoRepository, IDbProvider
    {
        DbConfig _dbConfig;
        public MongoDbProvider(DbConfig dbConfig) : base(dbConfig.ConnectionString, dbConfig.DatabaseName)
        {
            _dbConfig = dbConfig;
        }

        private IMongoCollection<T> CreateCollectionIfNotExists<T>(string collectionName)
        {
            var collection = MongoDbContext.Database.GetCollection<T>(collectionName);
            if(collection == null){
                MongoDbContext.Database.CreateCollection(collectionName);
                collection = MongoDbContext.Database.GetCollection<T>(collectionName);
            }
            return collection;
        }

        public async Task<IDbEntity> CreateDocumentAsync(string collectionName, IDbEntity document)
        {
            var collection = CreateCollectionIfNotExists<IDbEntity>(collectionName);
            document.Id = Guid.NewGuid().ToString();
            await collection.InsertOneAsync(document);
            return document;
        }

        public async Task<IDbEntity> ReadDocumentAsync(string collectionName, string documentId)
        {
            var collection = CreateCollectionIfNotExists<IDbEntity>(collectionName);
            var filter = Builders<IDbEntity>.Filter.Eq("_id", documentId);
            var found = await collection.FindAsync(filter);
            return found.FirstOrDefault();
        }

        public async Task<IDbEntity> ReplaceDocumentAsync(string collectionName, IDbEntity document)
        {
            var collection = CreateCollectionIfNotExists<IDbEntity>(collectionName);
            var filter = Builders<IDbEntity>.Filter.Eq("_id", document.Id);
            var found = await collection.FindOneAndReplaceAsync(filter, document);
            return found;
        }

        public async Task<IQueryable<T>> Search<T>(string collectionName, Expression<Func<T, bool>> predicate)
        {
            var collection = CreateCollectionIfNotExists<T>(collectionName);
            var s = await collection.FindAsync<T>(predicate);
            return s.ToList().Select(x => (T)x).AsQueryable<T>();;
        }

    }
}
