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
    public class ExpensePartTakerTests
    {
        private readonly Mock<IMapper> _autoMapper;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IHttpContextService> _identityService;
        private readonly Mock<IUserRepository> _userRepository;

        private const int sampleUserId = 1;
        private const int sampleEventId = 1;
        private const int sampleExpnseId = 1;
        private const int sampleCategoryId = 1;
        private const string sampleEventName = "SampleEventName";
        private const string sampleExpenseName = "SampleExpenseName";
        private const double sampleShareAmount = 20.4;
        private const double sampleTotalPrice = 100.5;
        public ExpensePartTakerTests()
        {
            _eventRepository = new Mock<IEventRepository>();
            _userRepository = new Mock<IUserRepository>();
            _autoMapper = new Mock<IMapper>();
            _identityService = new Mock<IHttpContextService>();
        }

        #region Domain
        [Theory]
        [InlineData(sampleUserId, sampleExpnseId, 0)]
        [InlineData(sampleUserId, sampleExpnseId, -1)]
        [InlineData(sampleUserId, 0, sampleShareAmount)]
        [InlineData(sampleUserId, -1, sampleShareAmount)]
        [InlineData(0, sampleExpnseId, sampleShareAmount)]
        [InlineData(-1, sampleExpnseId, sampleShareAmount)]
        public void CreateExpensePartTaker_ShouldThrowDomainException_WhenEventNotFound(int partTakerId, int expenseId, double shareAmount)
        {
            Assert.Throws<ExpenseManagerDomainException>(() => ExpensePartTaker.Create(partTakerId, expenseId, shareAmount));
        }

        [Fact]
        public void CreateExpensePartTaker_ShouldWork_WhenPropertiesAreValid()
        {
            ExpensePartTaker expensePartTaker = ExpensePartTaker.Create(sampleUserId, sampleExpnseId, sampleShareAmount);
            Assert.Equal(sampleUserId, expensePartTaker.PartTakerId);
            Assert.Equal(sampleExpnseId, expensePartTaker.ExpenseId);
            Assert.Equal(sampleShareAmount, expensePartTaker.ShareAmount);
        }

        #endregion Domain

        #region ApplicationService

        [Fact]
        public async Task AddExpensePartTaker_ShouldThrowApplicationServiceException_WhenEventNotFound()
        {
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = sampleExpnseId, UserId = sampleUserId, ShareAmount = sampleShareAmount };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult<Event>(null));
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);
          
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Event not found.", exception.Message);
        }


        [Fact]
        public async Task AddExpensePartTaker_ShouldThrowApplicationServiceException_WhenEventExpencesNotFound()
        {
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = sampleExpnseId, UserId = sampleUserId, ShareAmount = sampleShareAmount };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            eventInstance.EventExpenses.Clear(); 
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("EventExpense not found.", exception.Message);
        }

        [Fact]
        public async Task AddExpensePartTaker_ShouldThrowApplicationServiceException_WhenCurrentUserIsNotExpensePayerOrExpenseCreator()
        {
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = 0, UserId = sampleUserId, ShareAmount = sampleShareAmount };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, DateTime.Now, sampleTotalPrice, sampleUserId + 1 , sampleUserId + 2, sampleCategoryId, sampleEventId);
            eventInstance.EventExpenses.Add(expense);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Only expense payer or expense creator is allowed to add parttaker.", exception.Message);
        }

        [Fact]
        public async Task AddExpensePartTaker_ShouldThrowApplicationServiceException_WhenPartTakerIsDuplicate()
        {
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = 0, UserId = sampleUserId, ShareAmount = sampleShareAmount };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, DateTime.Now, sampleTotalPrice, sampleUserId , sampleUserId , sampleCategoryId, sampleEventId);
            ExpensePartTaker partTaker = ExpensePartTaker.Create(sampleUserId, sampleExpnseId, sampleShareAmount);
            expense.ExpensePartTakers.Add(partTaker);
            eventInstance.EventExpenses.Add(expense);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("PartTakerId is duplicated.", exception.Message);
        }


        [Theory]
        [InlineData(sampleTotalPrice - sampleShareAmount + 10)]
        [InlineData(double.MaxValue)]
        public async Task AddExpensePartTaker_ShouldThrowApplicationServiceException_WhenSumOfShareAmountIsGreaterThanTotalPrice(double shareAmount)
        {
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = 0, UserId = sampleUserId, ShareAmount = shareAmount };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, DateTime.Now, sampleTotalPrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId);
            ExpensePartTaker partTaker = ExpensePartTaker.Create(sampleUserId + 1 , sampleExpnseId, sampleShareAmount);
            expense.ExpensePartTakers.Add(partTaker);
            eventInstance.EventExpenses.Add(expense);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Sum of share amounts is greater than total price", exception.Message);
        }

        [Theory]
        [InlineData(sampleTotalPrice + 10)]
        [InlineData(double.MaxValue)]
        public async Task AddExpensePartTaker_ShouldThrowApplicationServiceException_WhenShareAmountIsGreaterThanTotalPrice(double shareAmount)
        {
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = 0, UserId = sampleUserId, ShareAmount = shareAmount };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, DateTime.Now, sampleTotalPrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId);
            eventInstance.EventExpenses.Add(expense);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Sum of share amounts is greater than total price", exception.Message);
        }

        [Fact]
        public async Task AddExpensePartTaker_Shouldwork_WhenParametersAreValid()
        {
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            eventInstance.Id = sampleEventId;
            var eventsList = new List<Event>();
            Expense expense = Expense.Create(sampleExpenseName, DateTime.Now, sampleTotalPrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId);
            expense.Id = sampleExpnseId;
            eventInstance.EventExpenses.Add(expense);
            var addCommand = new AddExpensePartTakerCommand() { EventId = sampleEventId, ExpenseId = sampleExpnseId, UserId = sampleUserId, ShareAmount = sampleShareAmount };
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _eventRepository.Setup(x => x.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
            .Callback((Event eventInstance, CancellationToken token, bool saveNow) =>
            {
                eventsList.Add(eventInstance);
            });
            _identityService.Setup(x => x.GetUserId()).Returns(() => 1);
            var handler = new AddExpensePartTakerCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object, _autoMapper.Object);

            await handler.Handle(addCommand, new CancellationToken());

            _eventRepository.Verify(x => x.UpdateAsync(It.IsAny<Event>(),It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
            var newPartTaker = eventsList.First().EventExpenses.First().ExpensePartTakers.First();
            Assert.Equal(sampleUserId, newPartTaker.PartTakerId);
            Assert.Equal(sampleShareAmount, newPartTaker.ShareAmount);
        }

        #endregion ApplicationService
    }
}
