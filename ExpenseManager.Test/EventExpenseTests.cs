using AutoMapper;
using ExpenseManager.ApplicationService.Commands.EventCommands;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Test
{
    public class EventExpenseTests
    {
        private readonly Mock<IMapper> _autoMapper;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IHttpContextService> _identityService;

        private const int sampleUserId = 1;
        private const int sampleEventId = 1;
        private const int sampleCategoryId = 1;
        private const double samplePrice = 100.5;
        private const string sampleEventName = "SampleEventName";
        private const string sampleExpenseName = "SampleExpenseName";
        public static readonly object[][] CorrectData =
        {
            new object[] { sampleExpenseName, DateTime.Now , double.MaxValue, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId},
            new object[] { sampleExpenseName, DateTime.MaxValue , double.MaxValue, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId},
            new object[] { sampleExpenseName, DateTime.MinValue , double.MaxValue, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId},
        };
        public EventExpenseTests()
        {
            _eventRepository = new Mock<IEventRepository>();
            _autoMapper = new Mock<IMapper>();
            _identityService = new Mock<IHttpContextService>();
        }

        #region Domain
      
        [Theory]
        [InlineData("", samplePrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId)]
        [InlineData(" ", samplePrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, 0, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, -1, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, 0, sampleUserId, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, -1, sampleUserId, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, sampleUserId, 0, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, sampleUserId, -1, sampleCategoryId, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, sampleUserId, sampleUserId, 0, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, sampleUserId, sampleUserId, -1, sampleEventId)]
        [InlineData(sampleExpenseName, samplePrice, sampleUserId, sampleUserId, sampleCategoryId, 0)]
        [InlineData(sampleExpenseName, samplePrice, sampleUserId, sampleUserId, sampleCategoryId, -1)]
        public void CreateExpense_ShouldThrowDomainException_WhenParametersAreInvalid(string name, double totalPrice, int payerId, int creatorId, int categoryId, int eventId)
        {
            Assert.Throws<ExpenseManagerDomainException>(() => Expense.Create(name, DateTime.Now, totalPrice, payerId, creatorId, categoryId, eventId));
        }

        [Fact]
        public void CreateExpense_ShouldCorrectTheName_WhenItContainsWhiteSpace()
        {
            string expenseName = " SampleExpense ";
            string expectedExpenseName = expenseName.Trim();

            Expense expense = Expense.Create(expenseName, DateTime.Now, samplePrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId);
            string actualEventName = expense.Name;

            Assert.Equal(actualEventName, expectedExpenseName);
        }

        [Theory, MemberData(nameof(CorrectData))]
        public void CreateExpense_ShouldWork_WhenPropertiesAreValid(string name, DateTime payDate, double totalPrice, int payerId, int creatorId, int categoryId, int eventId)
        {
            Expense expense = Expense.Create(name, payDate, totalPrice, payerId, creatorId, categoryId, eventId);

            Assert.Equal(name, expense.Name);
            Assert.Equal(payDate, expense.PayDateTime);
            Assert.Equal(totalPrice, expense.TotalPrice);
            Assert.Equal(payerId, expense.PayerId);
            Assert.Equal(creatorId, expense.CreatorId);
            Assert.Equal(categoryId, expense.CategoryId);
            Assert.Equal(eventId, expense.EventId);
        }
        #endregion Domain

        #region ApplicationService

      
        [Fact]
        public async Task AddExpense_ShouldThrowApplicationServiceException_WhenEventNotFound()
        {
            var addCommand = new AddExpenseCommand() { CategoryId = sampleCategoryId, EventId = sampleEventId, Name = sampleExpenseName, PayerId = sampleUserId, TotalPrice = samplePrice };
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _eventRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).Returns(() => Task.FromResult<Event>(null));
            var handler = new AddExpenseCommandHandler(_eventRepository.Object, _identityService.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Event not found.", exception.Message);
        }

        [Fact]
        public async Task AddExpense_ShouldWork_WhenParametersAreValid()
        {
            var addCommand = new AddExpenseCommand() { CategoryId = sampleCategoryId, EventId = sampleEventId, Name = sampleExpenseName, PayerId = sampleUserId, TotalPrice = samplePrice };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            var eventsList = new List<Event>() { eventInstance };
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _eventRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(() => eventInstance);
            _eventRepository.Setup(x => x.AddExpenceAsync(It.IsAny<Event>(), It.IsAny<Expense>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                 .Callback((Event eventInstance, Expense expense, CancellationToken token, bool saveNow) =>
                 {
                     eventsList.First().EventExpenses.Add(expense);
                 });
            var handler = new AddExpenseCommandHandler(_eventRepository.Object, _identityService.Object, _autoMapper.Object);

            EventDto actual = await handler.Handle(addCommand, new CancellationToken());
            Assert.True(eventsList.First().EventExpenses.Any());
            Assert.Equal(sampleExpenseName, eventsList.First().EventExpenses.Last().Name);
            _eventRepository.Verify(x => x.AddExpenceAsync(It.IsAny<Event>(), It.IsAny<Expense>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
        }
        #endregion ApplicationService
    }
}
