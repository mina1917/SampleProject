using MediatR;

namespace SampleProject.Application.Contracts
{
    public class ITransactionRequest<TResponse> : IRequest<TResponse>
    {
    }
    public interface ITransactionRequest : IRequest
    {
    }
}
