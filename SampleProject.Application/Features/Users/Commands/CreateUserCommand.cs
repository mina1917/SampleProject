using FluentValidation;
using MediatR;
using SampleProject.Application.Contracts;
using SampleProject.Domain.Users;
using SampleProject.Framework.Contracts;
using SampleProject.Messaging.Senders;

namespace SampleProject.Application.Features.Users.Commands
{
    public class CreateUserCommand : ITransactionRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        private class Handler : IRequestHandler<CreateUserCommand>
        {
            private readonly IWriteRepository<User> _userWriteRepository;
            private readonly IUserCreateSender _messagePublisher;

            public Handler(IWriteRepository<User> userWriteRepository)
            {
                _userWriteRepository = userWriteRepository;
            }

            public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new User(request.UserName, request.Password, request.Email);
                await _userWriteRepository.AddAsync(user, cancellationToken);
                await _userWriteRepository.SaveChangeAsync(cancellationToken);
                return Unit.Value;
            }
        }

        public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserCommandValidator()
            {
                RuleFor(p => p.UserName)
                    .NotEmpty().WithMessage(" نام کاربری الزامی میباشد");

                RuleFor(p => p.Password)
                    .NotEmpty().WithMessage(" کلمه عبور الزامی میباشد");

                RuleFor(p => p.Email)
                    .NotEmpty().WithMessage(" ایمیل الزامی میباشد");
            }
        }
    }
}
