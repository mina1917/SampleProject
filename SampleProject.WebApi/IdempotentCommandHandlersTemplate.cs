using Newtonsoft.Json;
using NServiceBus;
using SampleProject.Application.Contracts;
using SampleProject.Domain.ServiceBusMessages;
using SampleProject.Framework;
using ILogger = Serilog.ILogger;

namespace SampleProject.WebApi
{
    public abstract class IdempotentCommandHandlersTemplate<T> : IHandleMessages<T> where T : Command
    {
        private T _message;
        protected IMessageHandlerContext MessageHandlerContext;
        private readonly IServiceBusMessagesService _serviceBusMessagesService;
        private readonly ILogger _logger;


        protected IdempotentCommandHandlersTemplate(IServiceBusMessagesService serviceBusMessagesService, ILogger logger)
        {
            _serviceBusMessagesService = serviceBusMessagesService;
            _logger = logger;
        }

        protected IdempotentCommandHandlersTemplate(IServiceBusMessagesService serviceBusMessagesService)
        {
            _serviceBusMessagesService = serviceBusMessagesService;
        }

        public async Task Handle(T message, IMessageHandlerContext context)
        {
            var handlerName = GetType();

            _logger?.Information($"{DateTime.UtcNow} : {GetType().Name} handling message with body {JsonConvert.SerializeObject(message)} ");
            _message = message;
            MessageHandlerContext = context;
            if (await CheckDuplicateEvent())
                return;
            await Do(_message);
            await AddEventId();
            _logger?.Information($"{DateTime.UtcNow} : {GetType().Name} handled message with Id : {context.MessageId}");
        }

        protected abstract Task Do(T message);

        private async Task<bool> CheckDuplicateEvent()
        {
            return await _serviceBusMessagesService.IsDuplicateProcessedMessage(MessageHandlerContext.MessageId, GetType().Name, CancellationToken.None);
        }

        private async Task AddEventId()
        {
            await _serviceBusMessagesService.AddAsync(new ProcessedMessage(MessageHandlerContext.MessageId, GetType().Name));
        }
    }
}