using MediatR;
using SampleProject.Application.Dto;
using SampleProject.Framework.Cosmos;
using SampleProject.Framework.Pagination;
using SampleProject.Query;

namespace SampleProject.Application.Features.Users.Queries
{
    public class GetAllUsersQuery : PaginationParameters, IRequest<List<UserDto>>
    {
        public class Handler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
        {
            private readonly UserCosmosRepository _userCosmosRepository;

            public Handler(UserCosmosRepository userRepository)
            {
                _userCosmosRepository = userRepository;
            }

            public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userCosmosRepository.GetItemAsyncUsingLinq<UserQuery>(x => 1 == 1, CancellationToken.None);
                var userDtos = users
                    .Select(c => new UserDto
                    {
                        Id = Guid.Parse(c.Id),
                        UserName = c.UserName,
                        Password = c.Password,
                        Email = c.Email,
                    }).ToList();

                return userDtos;
            }
        }
    }
}
