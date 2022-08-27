using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NServiceBus;
using SampleProject.Domain.ServiceBusMessages;
using SampleProject.Framework;
using SampleProject.Framework.NServiceBus;
using SampleProject.Framework.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace SampleProject.Application.Behaviours
{
    public class ServiceBusSender : IServiceBusSender
    {
        private readonly IMessageSession _messageSession;
        private readonly string _connectionString;
        private readonly ILogger<ServiceBusSender> _logger;

        public ServiceBusSender(IConfiguration configuration, IMessageSession messageSession,
            ILogger<ServiceBusSender> logger)
        {
            _messageSession = messageSession;
            _logger = logger;
            _connectionString = GetConnectionString(configuration);
        }

        public async Task PublishMessage<T>(T domainEvent) where T : DomainEvent
        {
            try
            {
                var eventId = Guid.NewGuid();
                var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
                var body = JsonConvert.SerializeObject(domainEvent, settings);
                var publishedEvent = new PublishedEvent(eventId, body, false, domainEvent.GetType().AssemblyQualifiedName);
                try
                {
                    var publishOptions = new PublishOptions();
                    publishOptions.SetMessageId(eventId.ToString());
                    await _messageSession.Publish(domainEvent, publishOptions);
                    publishedEvent.SetAsPublished();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"event {eventId} had an error");
                }

                Insert(publishedEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SendMessage<T>(T command, TimeSpan timeSpan) where T : Command
        {
            var eventId = Guid.NewGuid();
            var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
            var body = JsonConvert.SerializeObject(command, settings);
            var publishedEvent = new PublishedEvent(eventId, body, false, command.GetType().AssemblyQualifiedName);
            try
            {
                var sendOptions = new SendOptions();
                sendOptions.DelayDeliveryWith(timeSpan);
                sendOptions.SetMessageId(eventId.ToString());
                sendOptions.SetDestination("Dashboard");
                await _messageSession.Send(command, sendOptions);
                publishedEvent.SetAsPublished();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"event {eventId} had an error");
            }

            Insert(publishedEvent);
        }

        public async Task SendMessage<T>(T command) where T : Command
        {
            var eventId = Guid.NewGuid();
            var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
            var body = JsonConvert.SerializeObject(command, settings);
            var publishedEvent = new PublishedEvent(eventId, body, false, command.GetType().AssemblyQualifiedName);
            try
            {
                var sendOptions = new SendOptions();
                sendOptions.SetMessageId(eventId.ToString());
                sendOptions.SetDestination("Dashboard");
                await _messageSession.Send(command, sendOptions);
                publishedEvent.SetAsPublished();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"event {eventId} had an error");
            }

            Insert(publishedEvent);
        }

        public void Insert(PublishedEvent publishedEvent)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlParameter body = new SqlParameter();
                body.ParameterName = "@body";
                body.SqlDbType = SqlDbType.NVarChar;
                body.Value = publishedEvent.Body;

                var query =
                    " INSERT INTO [PublishedEvents] ([EventId],[EventType],[Body],[PublishedOn],[IsPublished])" +
                    $" Values('{publishedEvent.EventId}',N'{publishedEvent.EventType}',@body,'{publishedEvent.PublishedOn}','{publishedEvent.IsPublished}')";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(body);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
        }

        private string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            return connectionString;
        }
    }
}