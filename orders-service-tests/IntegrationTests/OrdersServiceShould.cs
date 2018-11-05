using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using OrdersService.Core;
using OrdersService.Core.Interfaces;
using OrdersService.Core.Models;
using OrdersService.Core.Services;
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
        IOrdersMessageBus _eventBus;
        IOrdersService _sut;
        ITimeProvider _timeProvider;
        IFixture _fixture;
        OrderBuilder _orderBuilder;
        
        public OrdersServiceShould()
        {
            var dbConfig = new Core.Config.DbConfig(){};
            _ordersRepository = new OrdersRepository(new InMemoryDbProvider(dbConfig));
            _eventBus = Substitute.For<IOrdersMessageBus>();
            _timeProvider = new UtcTimeProvider();
            _sut = new Core.OrdersService(_ordersRepository, _eventBus, _timeProvider);
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _orderBuilder = new OrderBuilder();
        }

        [Fact]
        public async Task GetExistingOrder()
        {
            //Arrange
            var expected = _orderBuilder.Build();
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
            var order = _orderBuilder.Build();
            order = await _ordersRepository.SaveAsync(order);
            order.Submit();

            //Act
            var actual = await _sut.Submit(order.Id);

            //Assert
            _eventBus.Received(1).WriteOrder(Arg.Is<Order>(x => x.Id == order.Id && x.Status == order.Status));
        }

        [Fact]
        public async Task CancelOrder()
        {
            //Arrange
            var order = _orderBuilder.Build();
            order = await _ordersRepository.SaveAsync(order);
            order.Cancel();

            //Act
            var actual = await _sut.Cancel(order.Id);

            //Assert
            _eventBus.Received(1).WriteOrder(Arg.Is<Order>(x => x.Id == order.Id && x.Status == order.Status));
        }

        [Fact]
        public async Task AddItemToOrder()
        {
            //Arrange
            var order = _orderBuilder.WithRandomId().Build();
            order = await _ordersRepository.SaveAsync(order);
            var item = _fixture.Create<OrderItem>();
            order.AddItem(item);
            
            //Act
            var actual = await _sut.AddItem(order.Id, item);

            //Assert
            _eventBus.Received(1).WriteOrder(Arg.Is<Order>(x => x.Id == order.Id && x.Items.Count == order.Items.Count));
        }

        [Fact]
        public async Task RemoveItemFromOrder()
        {
            //Arrange
            var order = _orderBuilder.WithRandomId().Build();
            order = await _ordersRepository.SaveAsync(order);
            var itemId = order.Items.First().ItemId;
            order.RemoveItem(itemId);

            //Act
            var actual = await _sut.RemoveItem(order.Id, itemId);

            //Assert
            _eventBus.Received(1).WriteOrder(Arg.Is<Order>(x => x.Id == order.Id && x.Items.Count == order.Items.Count));
        }

        [Fact]
        public async Task ListOrdersByUser()
        {
            //Arrange
            var order1 = await _ordersRepository.SaveAsync(_orderBuilder.WithRandomId().WithUserId("testByUserId").Build());
            var order2 = await _ordersRepository.SaveAsync(_orderBuilder.WithRandomId().WithUserId("testByUserId").Build());
            var list = new List<Order>() { order1, order2 };
            
            //Act
            var actual = await _sut.ListByUser(order1.UserId);

            //Assert
            actual.Should().BeEquivalentTo(list);
        }

    }

}
