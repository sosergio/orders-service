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
        Order _order { get; set; }
        public OrderBuilder()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            WithDefaultValues();
        }

        public OrderBuilder WithDefaultValues()
        {
            var refId = "ref-id";
            var userId = "user-id";
            var items = _fixture.Create<List<OrderItem>>();
            _order = new Order(refId, userId, firstOfJan, items);
            return this;
        }

        public OrderBuilder WithRandomId()
        {
            return WithGivenId(Guid.NewGuid().ToString());
        }

        public OrderBuilder WithGivenId(string id)
        {
            _order.Id = id;
            return this;
        }

        public OrderBuilder WithUserId(string v)
        {
            _order.UserId = v;
            return this;
        }

        public Order Build()
        {
            return _order;
        }
    }
}
