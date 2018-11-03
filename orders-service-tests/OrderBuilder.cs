using AutoFixture;
using AutoFixture.AutoNSubstitute;
using OrdersService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Tests
{
    public class OrderBuilder
    {
        IFixture _fixture;
        DateTimeOffset firstOfJan = new DateTimeOffset(new DateTime(2018, 01, 01));
        
        public OrderBuilder()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        public Order WithDefaultValues()
        {
            var refId = "ref-id";
            var userId = "user-id";
            var items = _fixture.Create<List<OrderItem>>();
            return new Order(refId, userId, firstOfJan, items);
        }

        public Order WithRandomId()
        {
            return WithGivenId(Guid.NewGuid().ToString());
        }

        public Order WithGivenId(string id)
        {
            var order = WithDefaultValues();
            order.Id = id;
            return order;
        }
    }
}
