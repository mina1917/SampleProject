using SampleProject.Application.Contracts;
using SampleProject.Framework.Cosmos;
using SampleProject.Framework.NServiceBus;
using SampleProject.Query;
using SampleProject.Query.DomainEvents;
using ILogger = Serilog.ILogger;

namespace SampleProject.WebApi.NServiceBusHandlers.Users
{
    public class UserCreatedHandler : IdempotentEventHandlersTemplate<UserCreated>
    {
        private readonly UserCosmosRepository _leadCosmosRepository;
        private readonly IServiceBusSender _serviceBusSender;

        public UserCreatedHandler(UserCosmosRepository leadCosmosRepository,
            IServiceBusMessagesService serviceBusMessagesService,
            ILogger logger,
            IServiceBusSender serviceBusSender) : base(
            serviceBusMessagesService, logger)
        {
            _leadCosmosRepository = leadCosmosRepository;
            _serviceBusSender = serviceBusSender;
        }

        protected override async Task Do(UserCreated message)
        {
            var userQuery = new UserQuery(message.Email, message.Email, message.Mobile);
            await _leadCosmosRepository.AddItemAsync(userQuery);
        }
    }
}