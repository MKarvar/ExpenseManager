using AutoMapper;
using ExpenseManager.ApplicationService.Commands.EventCommands;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Test
{
    public class EventTests
    {
        private readonly Mock<IMapper> _autoMapper;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IHttpContextService> _identityService;
        private readonly Mock<IUserRepository> _userRepository;

        private const int sampleUserId = 1;
        private const int sampleEventId = 1;
        private const int sampleCategoryId = 1;
        private const int sampleExpenseId = 1;
        private const string sampleEventName = "SampleEventName";
        private const string sampleExpenseName = "SampleExpenseName";
        private const string samplePassword = "SamplePassword";
        private const string sampleUsername = "SampleUsername";
        private const double sampleTotalPrice = 100.2;
        private const double sampleShareAmount = 20.5;
        public EventTests()
        {
            _eventRepository = new Mock<IEventRepository>();
            _autoMapper = new Mock<IMapper>();
            _identityService = new Mock<IHttpContextService>();
            _userRepository = new Mock<IUserRepository>();
        }

        #region Domain
        [Theory]
        [InlineData("", sampleUserId)]
        [InlineData(" ", sampleUserId)]
        [InlineData(" ", 0)]
        [InlineData(sampleEventName, -1)]
        public void CreateEvent_ShouldThrowDomainException_WhenParametersAreInvalid(string Name, int creatorId)
        {
            Assert.Throws<ExpenseManagerDomainException>(() => Event.Create(Name, creatorId));
        }

        [Fact]
        public void CreateEvent_ShouldThrowDomainException_WhenNameIsNull()
        {
            Assert.Throws<ExpenseManagerDomainException>(() => Event.Create(null, sampleUserId));
        }


        [Fact]
        public void CreateEvent_ShouldCorrectTheName_WhenItContainsWhiteSpace()
        {
            string eventName = " SampleEvent ";
            string expectedEventName = eventName.Trim();

            Event eventInstance = Event.Create(eventName, sampleUserId);
            string actualEventName = eventInstance.Name;

            Assert.Equal(actualEventName, expectedEventName);
        }

        #endregion Domain

        #region ApplicationService

        [Fact]
        public async Task AddEvent_ShouldWork_WhenParametersAreValid()
        {
            var addEventCommand = new AddEventCommand() { Name = sampleEventName };
            var eventsList = new List<Event>();
            var expected = new EventDto() { Name = sampleEventName };
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _eventRepository.Setup(x => x.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
               .Callback((Event eventInstance, CancellationToken token, bool saveNow) =>
               {
                   eventsList.Add(eventInstance);
               });
            var handler = new AddEventCommandHandler(_eventRepository.Object, _identityService.Object, _autoMapper.Object);

            EventDto actual = await handler.Handle(addEventCommand, new CancellationToken());

            Assert.True(eventsList.Any());
            Assert.Equal(expected.Name, eventsList.Last().Name);
            _eventRepository.Verify(x => x.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
        }


        [Fact]
        public async Task EditEvent_ShouldThrowApplicationServiceException_WhenEventNotFound()
        {
            var addCommand = new EditEventCommand() { Name = sampleEventName, Id = sampleEventId };
            _eventRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).Returns(() => Task.FromResult<Event>(null));
            var handler = new EditEventCommandHandler(_eventRepository.Object, _identityService.Object, _autoMapper.Object);

            _eventRepository.Verify(x => x.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Event not found.", exception.Message);
        }

        [Fact]
        public async Task EditEvent_ShouldWork_WhenParametersAreValid()
        {
            var addCommand = new EditEventCommand() { Name = sampleEventName, Id = sampleEventId };
            Event eventInstance = Event.Create(sampleEventName + "Old", sampleUserId);
            _eventRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(() => eventInstance);
            _eventRepository.Setup(x => x.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()));
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new EditEventCommandHandler(_eventRepository.Object, _identityService.Object, _autoMapper.Object);

            EventDto eventDto = await handler.Handle(addCommand, new CancellationToken());

            _eventRepository.Verify(x => x.UpdateAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
            Assert.Equal(sampleEventName, eventInstance.Name);
        }

        [Fact]
        public async Task RemoveEvent_ShouldThrowApplicationServiceException_WhenUserNotFound()
        {
            var removeCommand = new RemoveEventCommand() { Id = sampleEventId };
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).Returns(() => Task.FromResult<User>(null));
            var handler = new RemoveEventCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object);

            _eventRepository.Verify(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(removeCommand, new CancellationToken()));
            Assert.Equal("User not found.", exception.Message); 
        }

        [Fact]
        public async Task RemoveEvent_ShouldThrowApplicationServiceException_WhenEventNotFound()
        {
            var removeCommand = new RemoveEventCommand() { Id = sampleEventId };
            User user = User.Create(sampleUsername, samplePassword);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(user);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult<Event>(null));
            var handler = new RemoveEventCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object);

            _eventRepository.Verify(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(removeCommand, new CancellationToken()));
            Assert.Equal("Event not found.", exception.Message);
        }

        [Fact]
        public async Task RemoveEvent_ShouldThrowApplicationServiceException_WhenEventCreatorIsNotCurrentUser()
        {
            var removeCommand = new RemoveEventCommand() { Id = sampleEventId };
            User user = User.Create(sampleUsername, samplePassword);
            Event eventInstance = Event.Create(sampleUsername, sampleUserId + 1);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(user);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(eventInstance);
            var handler = new RemoveEventCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object);

            _eventRepository.Verify(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(removeCommand, new CancellationToken()));
            Assert.Equal("Only event creator is allowed to remove it.", exception.Message);
        }

        [Fact]
        public async Task RemoveEvent_ShouldThrowApplicationServiceException_WhenEventHasExpensesCreatedByOtherUser()
        {
            var removeCommand = new RemoveEventCommand() { Id = sampleEventId };
            User user = User.Create(sampleUsername, samplePassword);
            Event eventInstance = Event.Create(sampleUsername, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, System.DateTime.Now, sampleTotalPrice, sampleUserId, sampleUserId + 1, sampleCategoryId, sampleEventId);
            eventInstance.EventExpenses.Add(expense);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(user);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(eventInstance);
            var handler = new RemoveEventCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object);

            _eventRepository.Verify(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(removeCommand, new CancellationToken()));
            Assert.Equal("Event includes expenses created by other users and con't be deleted.", exception.Message);
        }

        [Fact]
        public async Task RemoveEvent_ShouldThrowApplicationServiceException_WhenEventHasExpensesThatIsSharePayedByByOtherUser()
        {
            var removeCommand = new RemoveEventCommand() { Id = sampleEventId };
            User user = User.Create(sampleUsername, samplePassword);
            Event eventInstance = Event.Create(sampleUsername, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, System.DateTime.Now, sampleTotalPrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId);
            ExpensePartTaker partTaker = ExpensePartTaker.Create(sampleUserId + 1, sampleExpenseId, sampleShareAmount);
            partTaker = ExpensePartTaker.Pay(partTaker);
            expense.ExpensePartTakers.Add(partTaker);
            eventInstance.EventExpenses.Add(expense);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(user);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(eventInstance);
            var handler = new RemoveEventCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object);

            _eventRepository.Verify(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Never);
            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(removeCommand, new CancellationToken()));
            Assert.Equal("Event includes paid expenses and can't be deleted.", exception.Message);
        }

        [Fact]
        public async Task RemoveEvent_ShouldWork_WhenParametersAreValid()
        {
            var removeCommand = new RemoveEventCommand() { Id = sampleEventId };
            User user = User.Create(sampleUsername, samplePassword);
            Event eventInstance = Event.Create(sampleUsername, sampleUserId);
            Expense expense = Expense.Create(sampleExpenseName, System.DateTime.Now, sampleTotalPrice, sampleUserId, sampleUserId, sampleCategoryId, sampleEventId);
            ExpensePartTaker partTaker = ExpensePartTaker.Create(sampleUserId, sampleExpenseId, sampleShareAmount);
            partTaker = ExpensePartTaker.Pay(partTaker);
            expense.ExpensePartTakers.Add(partTaker);
            eventInstance.EventExpenses.Add(expense);
            List<Event> events = new List<Event>() { eventInstance };
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(user);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(eventInstance);
            var handler = new RemoveEventCommandHandler(_eventRepository.Object, _userRepository.Object, _identityService.Object);
            _eventRepository.Setup(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
             .Callback((Event eventInstance, CancellationToken token, bool saveNow) =>
             {
                 events.Remove(eventInstance);
             });

            await handler.Handle(removeCommand, new CancellationToken());

            Assert.False(events.Any());
            _eventRepository.Verify(x => x.DeleteEventAndChildrenAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
        }
        #endregion ApplicationService
    }
}
