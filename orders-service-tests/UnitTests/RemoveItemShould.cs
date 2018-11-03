using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OrdersService.Core;
using OrdersService.Core.Models;
using OrdersService.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using OrdersService.Core.Models.Errors;

namespace OrdersService.Tests.UnitTests
{
    public class RemoveItemShould
    {
        IRepository<Order> _ordersRepository;
        IMessageBusProvider _queue;
        IOrdersService _sut;
        ITimeProvider _timeProvider;
        IFixture _fixture;
        OrderBuilder _orderBuilder;

        public RemoveItemShould()
        {
            _ordersRepository = Substitute.For<IRepository<Order>>();
            _queue = Substitute.For<IMessageBusProvider>();
            _timeProvider = Substitute.For<ITimeProvider>();
            _sut = new OrdersService.Core.OrdersService(_ordersRepository, _queue, _timeProvider);
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _orderBuilder = new OrderBuilder();
        }

        [Fact]
        public async Task RemoveAndSaveInRepository()
        {
            //Arrange
            var order = _orderBuilder.WithRandomId();
            _timeProvider.Now().Returns(order.Date);
            _ordersRepository.LoadById(order.Id).Returns(order);
            var expected = order;
            expected.RemoveItem(expected.Items.First().Id);
            _ordersRepository.SaveAsync(expected).Returns(expected);

            //Act
            var result = await _sut.RemoveItem(order.Id, order.Items.First().Id);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(" ", typeof(NotFoundError))]
        public async Task Throws_WhenItemIsInvalid(string s, Type excType)
        {
            var order = _orderBuilder.WithRandomId();
            try
            {
                await _sut.RemoveItem(order.Id, s);
            }
            catch (Exception exc)
            {
                Assert.Equal(excType.Name, exc.GetType().Name);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(" ", typeof(NotFoundError))]
        public async Task Throws_WhenOrderIsInvalid(string s, Type excType)
        {
            var order = _orderBuilder.WithRandomId();
            try
            {
                await _sut.RemoveItem(s, order.Items.First().Id);
            }
            catch (Exception exc)
            {
                Assert.Equal(excType.Name, exc.GetType().Name);
            }
        }


    }

}
