using SampleProject.Domain.ServiceBusMessages;

namespace SampleProject.Application.Contracts
{
    public interface IServiceBusMessagesService
    {
        Task<bool> IsDuplicateProcessedMessage(string eventId, string handlerName, CancellationToken cancellationToken);
        Task AddAsync(ProcessedMessage processedMessage);
    }
}