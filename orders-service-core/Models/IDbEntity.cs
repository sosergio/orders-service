using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDbGenericRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.DataModels
{
    public interface IDbEntity: IDocument<string>
    {
        
    }
}
