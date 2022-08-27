using MediatR;
using SampleProject.Application.Exceptions;
using SampleProject.Application.Features.Users.Queries;
using SampleProject.Domain.Users;
using SampleProject.Framework.Contracts;

namespace SampleProject.Application.Features.Users.Commands
{
    public class RemoveUserCommand : IRequest
    {
        public Guid Id { get; set; }

        private class Handler : IRequestHandler<RemoveUserCommand>
        {
            private readonly IWriteRepository<User> _userWriteRepository;
            private readonly IMediator _mediator;

            public Handler(
                IWriteRepository<User> userRepository,
                IMediator mediator)
            {
                _userWriteRepository = userRepository;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _mediator.Send(new GetUserByIdQuery() { Id = request.Id });

                if (user == null)
                    throw new AppException("اطلاعات یافت نشد");

                await _userWriteRepository.DeleteAsync(user);
                await _userWriteRepository.SaveChangeAsync();

                return Unit.Value;
            }
        }
    }
}