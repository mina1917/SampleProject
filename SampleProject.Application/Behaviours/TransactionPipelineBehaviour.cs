using MediatR;
using SampleProject.Application.Contracts;
using System.Transactions;

namespace SampleProject.Application.Behaviours
{
    public class TransactionPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITransactionRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ITransactionRequest<TResponse> || request is ITransactionRequest)
            {
                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.MaximumTimeout
                };

                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var resp = await next();
                    tran.Complete();
                    return resp;
                }
            }

            return await next();
        }
    }
}
