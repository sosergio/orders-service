using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OrdersService.Core;
using OrdersService.Core.Models;
using OrdersService.Core.Models.Errors;
using OrdersService.Core.Interfaces;
using System.Threading.Tasks;
using Xunit;
using OrdersService.Core.Services;

namespace OrdersService.Tests.UnitTests
{
    public class CreateOrderShould
    {
        IRepository<Order> _ordersRepository;
        IOrdersMessageBus _eventBus;
        IOrdersService _sut;
        ITimeProvider _timeProvider;
        IFixture _fixture;
        OrderBuilder _orderBuilder;
        
        public CreateOrderShould()
        {
            _ordersRepository = Substitute.For<IRepository<Order>>();
            _eventBus = Substitute.For<IOrdersMessageBus>();
            _timeProvider = Substitute.For<ITimeProvider>();
            _sut = new OrdersService.Core.OrdersService(_ordersRepository, _eventBus, _timeProvider);
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _orderBuilder = new OrderBuilder();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Throws_WhenReferenceIsInvalid(string s)
        {
            var order = _orderBuilder.Build();
            (await _sut.Create(s, order.UserId, order.Items)).Should().Throws<ApplicationError>();    
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Throws_WhenUserIdIsInvalid(string s)
        {
            var order = _orderBuilder.Build();
            (await _sut.Create(order.Reference, null, order.Items)).Should().Throws<ApplicationError>();
        }
        

    }

}
