using Newtonsoft.Json;
using NServiceBus;
using SampleProject.Application.Contracts;
using SampleProject.Domain.ServiceBusMessages;
using SampleProject.Framework;
using ILogger = Serilog.ILogger;

namespace SampleProject.WebApi
{
    public abstract class IdempotentEventHandlersTemplate<T> : IHandleMessages<T> where T : DomainEvent
    {
        private T _message;
        private IMessageHandlerContext _messageHandlerContext;
        private readonly IServiceBusMessagesService _serviceBusMessagesService;
        private readonly ILogger _logger;
        protected IdempotentEventHandlersTemplate(IServiceBusMessagesService serviceBusMessagesService, ILogger logger)
        {
            _serviceBusMessagesService = serviceBusMessagesService;
            _logger = logger;
        }

        protected IdempotentEventHandlersTemplate(IServiceBusMessagesService serviceBusMessagesService)
        {
            _serviceBusMessagesService = serviceBusMessagesService;
        }

        private void SetConfig()
        {
            //var configuration = AzureAppConfigIHostBuilderExtensions.BuildConfiguration();
            //var dashboardConfig = new DashboardConfig();
            //configuration.GetSection("DashboardConfig").Bind(dashboardConfig);
        }

        public async Task Handle(T message, IMessageHandlerContext context)
        {
            var handlerName = GetType();
            _message = message;
            _messageHandlerContext = context;
            if (await CheckDuplicateEvent())
                return;
            _logger?.Information($"{DateTime.UtcNow} : {handlerName} handling message with body {JsonConvert.SerializeObject(message)} ");
            await Do(_message);
            await AddEventId();
            _logger?.Information($"{DateTime.UtcNow} : {handlerName} handled message with Id : {context.MessageId}");
        }

        protected abstract Task Do(T message);

        private async Task<bool> CheckDuplicateEvent()
        {
            return await _serviceBusMessagesService.IsDuplicateProcessedMessage(_messageHandlerContext.MessageId, GetType().Name, CancellationToken.None);
        }

        private async Task AddEventId()
        {
            await _serviceBusMessagesService.AddAsync(new ProcessedMessage(_messageHandlerContext.MessageId, GetType().Name));
        }

        public async Task InvokeClearCache(Task clearCache)
        {
            try
            {
                if (clearCache == null)
                    return;
                await clearCache;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}