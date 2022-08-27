using MediatR;
using SampleProject.Domain.Users;
using SampleProject.Framework.Cosmos;
using SampleProject.Query;

namespace SampleProject.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public Guid Id { get; set; }

        public class Handler : IRequestHandler<GetUserByIdQuery, User>
        {
            private readonly UserCosmosRepository _userCosmosRepository;

            public Handler(UserCosmosRepository userRepository)
            {
                _userCosmosRepository = userRepository;
            }

            public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var user = await _userCosmosRepository.GetFirstItemAsyncUsingLinq<UserQuery>(x => x.Id == request.Id.ToString(), cancellationToken);
                return new User(Guid.Parse(user.Id), user.UserName, user.Password, user.Email);
            }
        }
    }
}
