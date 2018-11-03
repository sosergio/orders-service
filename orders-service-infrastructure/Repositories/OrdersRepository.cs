using Ardalis.GuardClauses;
using OrdersService.Core.Models;
using OrdersService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Repositories
{
    public class OrdersRepository : IRepository<Order>
    {
        IDbProvider _dbProvider;
        public string CollectionId { get; } = "Orders";

        public OrdersRepository(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }
        public void Delete(string itemId)
        {
            Guard.Against.NullOrEmpty(itemId, nameof(itemId));
            _dbProvider.ReadDocumentAsync(CollectionId, itemId);
        }

        public async Task<Order> SaveAsync(Order item)
        {
            if (String.IsNullOrEmpty(item.Id))
            {
                 return (Order)await _dbProvider.CreateDocumentAsync(CollectionId, item);
            }
            else
            {
                return (Order)await _dbProvider.ReplaceDocumentAsync(CollectionId, item);
            }
        }

        public async Task<IEnumerable<Order>> Search(Expression<Func<Order, bool>> predicate)
        {
            return await Task.FromResult(_dbProvider.Search<Order>(CollectionId).Where(predicate));
        }

        public async Task<Order> LoadById(string itemId)
        {
            Guard.Against.NullOrEmpty(itemId, nameof(itemId));
            return await Task.FromResult(_dbProvider.Search<Order>(CollectionId).SingleOrDefault(x => x.Id == itemId));
        }
    }
}
