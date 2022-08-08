using AutoMapper;
using ExpenseManager.ApplicationService.Commands.EventCommands;
using ExpenseManager.ApplicationService.Contracts;
using ExpenseManager.ApplicationService.Dtos.UserDtos;
using ExpenseManager.ApplicationService.Exceptions;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.Exceptions;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Test
{
    public class EventParticipantTests
    {
        private readonly Mock<IMapper> _autoMapper;
        private readonly Mock<IEventRepository> _eventRepository;
        private readonly Mock<IHttpContextService> _identityService;
        private readonly Mock<IUserRepository> _userRepository;

        private const int sampleUserId = 1;
        private const int sampleEventId = 1;
        private const string sampleEventName = "SampleEventName";
        public EventParticipantTests()
        {
            _eventRepository = new Mock<IEventRepository>();
            _autoMapper = new Mock<IMapper>();
            _identityService = new Mock<IHttpContextService>();
            _userRepository = new Mock<IUserRepository>();
        }

        #region Domain
     

        [Theory]
        [InlineData(sampleUserId, -1)]
        [InlineData(0, sampleEventId)]
        [InlineData(-1, sampleEventId)]
        [InlineData(-1,0)]
        public void CreateEventParticipant_ShouldThrowDomainException_WhenParametersAreInvalid(int participantId, int eventId)
        {
            Assert.Throws<ExpenseManagerDomainException>(() => EventParticipant.Create(participantId, eventId));
        }

        [Fact]
        public void CreateEventParticipant_Shouldwork_WhenParametersArevalid()
        {
           EventParticipant eventParticipant = EventParticipant.Create(sampleUserId, sampleEventId);

            Assert.Equal(sampleEventId, eventParticipant.EventId);
            Assert.Equal(sampleUserId, eventParticipant.ParticipantId);
        }

        #endregion Domain

        #region ApplicationService
        [Fact]
        public async Task AddParticipant_ShouldThrowApplicationServiceException_WhenEventNotFound()
        {
            var addCommand = new AddParticipantCommand() { EventId = sampleEventId, UserId = sampleUserId };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult<Event>(null));
            var handler = new AddParticipantCommandHandler(_eventRepository.Object, _identityService.Object, _userRepository.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Event not found.", exception.Message);
        }

        [Fact]
        public async Task AddParticipant_ShouldThrowApplicationServiceException_WhenParticipantIdIsDuplicate()
        {
            var addCommand = new AddParticipantCommand() { EventId = sampleEventId, UserId = sampleUserId};
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            EventParticipant participant = EventParticipant.Create(sampleUserId, sampleEventId);
            eventInstance.EventParticipants.Add(participant);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new AddParticipantCommandHandler(_eventRepository.Object, _identityService.Object, _userRepository.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("ParticipantId is duplicated.", exception.Message);
        }


        [Fact]
        public async Task AddParticipant_ShouldThrowApplicationServiceException_WhenEventCreatorIsNotCurrentUser()
        {
            var addCommand = new AddParticipantCommand() { EventId = sampleEventId, UserId = sampleUserId };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId + 1);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            var handler = new AddParticipantCommandHandler(_eventRepository.Object, _identityService.Object, _userRepository.Object, _autoMapper.Object);

            var exception = await Assert.ThrowsAsync<ExpenseManagerApplicationServiceException>(async () => await handler.Handle(addCommand, new CancellationToken()));
            Assert.Equal("Only event creator is allowed to add participants.", exception.Message);
        }

        [Fact]
        public async Task AddParticipant_ShouldWork_WhenParametersAreValid()
        {
            var addCommand = new AddParticipantCommand() { EventId = sampleEventId, UserId = sampleUserId };
            Event eventInstance = Event.Create(sampleEventName, sampleUserId);
            _eventRepository.Setup(x => x.GetByIdIncludingChildrenAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => eventInstance);
            _identityService.Setup(x => x.GetUserId()).Returns(() => sampleUserId);
            _eventRepository.Setup(x => x.AddParticipantAsync(It.IsAny<Event>(), It.IsAny<EventParticipant>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
               .Callback((Event eventInstance, EventParticipant particicpant, CancellationToken token, bool saveNow) =>
               {
                   eventInstance.EventParticipants.Add(particicpant);
               });
            _eventRepository.Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), It.IsAny<int>())).ReturnsAsync(() => eventInstance);
            var handler = new AddParticipantCommandHandler(_eventRepository.Object, _identityService.Object, _userRepository.Object, _autoMapper.Object);

            EventDto eventDto = await handler.Handle(addCommand, new CancellationToken());

            _eventRepository.Verify(x => x.AddParticipantAsync(It.IsAny<Event>(), It.IsAny<EventParticipant>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()), Times.Once);
            Assert.True(eventInstance.EventParticipants.Any());
            Assert.Equal(sampleUserId, eventInstance.EventParticipants.First().ParticipantId);
            Assert.Equal(sampleEventId, eventInstance.EventParticipants.First().EventId);
        }

        #endregion ApplicationService
    }
}
