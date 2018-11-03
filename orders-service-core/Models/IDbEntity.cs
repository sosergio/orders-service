using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.DataModels
{
    public interface IDbEntity
    {
        string Id { get; set; }
    }
}
