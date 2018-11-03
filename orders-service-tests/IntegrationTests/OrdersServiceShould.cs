using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using OrdersService.Core;
using OrdersService.Core.Interfaces;
using OrdersService.Core.Models;
using OrdersService.Infrastructure.Providers;
using OrdersService.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OrdersService.Tests.IntegrationTests
{
    public class OrdersServiceShould
    {
        IRepository<Core.Models.Order> _ordersRepository;
        IMessageBusProvider _queue;
        IOrdersService _sut;
        ITimeProvider _timeProvider;
        IFixture _fixture;
        OrderBuilder _orderBuilder;
        
        public OrdersServiceShould()
        {
            _ordersRepository = new OrdersRepository(new InMemoryDbProvider());
            _queue = Substitute.For<IMessageBusProvider>();
            _timeProvider = new UtcTimeProvider();
            _sut = new Core.OrdersService(_ordersRepository, _queue, _timeProvider);
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _orderBuilder = new OrderBuilder();
        }

        [Fact]
        public async Task GetExistingOrder()
        {
            //Arrange
            var expected = _orderBuilder.WithDefaultValues();
            expected = await _ordersRepository.SaveAsync(expected);
            
            //Act
            var actual = await _sut.LoadById(expected.Id);
            
            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task SubmitOrder()
        {
            //Arrange
            var order = _orderBuilder.WithDefaultValues();
            order = await _ordersRepository.SaveAsync(order);
            order.Submit();

            //Act
            var actual = await _sut.Submit(order.Id);

            //Assert
            actual.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task CancelOrder()
        {
            //Arrange
            var order = _orderBuilder.WithDefaultValues();
            order = await _ordersRepository.SaveAsync(order);
            order.Cancel();

            //Act
            var actual = await _sut.Cancel(order.Id);

            //Assert
            actual.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task AddItemToOrder()
        {
            //Arrange
            var order = _orderBuilder.WithDefaultValues();
            order = await _ordersRepository.SaveAsync(order);
            var item = _fixture.Create<OrderItem>();
            order.AddItem(item);

            //Act
            var actual = await _sut.AddItem(order.Id, item);

            //Assert
            actual.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task RemoveItemFromOrder()
        {
            //Arrange
            var order = _orderBuilder.WithDefaultValues();
            order = await _ordersRepository.SaveAsync(order);
            var itemId = order.Items.First().Id;
            order.RemoveItem(itemId);

            //Act
            var actual = await _sut.RemoveItem(order.Id, itemId);

            //Assert
            actual.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task ListOrdersByUser()
        {
            //Arrange
            var order1 = await _ordersRepository.SaveAsync(_orderBuilder.WithUserId("testByUserId"));
            var order2 = await _ordersRepository.SaveAsync(_orderBuilder.WithUserId("testByUserId"));
            var list = new List<Order>() { order1, order2 };
            
            //Act
            var actual = await _sut.ListByUser(order1.UserId);

            //Assert
            actual.Should().BeEquivalentTo(list);
        }

    }

}
