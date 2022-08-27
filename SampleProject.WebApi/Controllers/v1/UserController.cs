using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleProject.Application.Dto;
using SampleProject.Application.Features.Users.Commands;
using SampleProject.Application.Features.Users.Queries;
using SampleProject.Framework.Pagination;
using SampleProject.WebApi.Endpoint;

namespace SampleProject.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class UserController : BaseApiControllerWithDefaultRoute
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ApiResult> Post([FromBody] CreateUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<ApiResult> Delete(RemoveUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        public async Task<ApiResult<PagedList<UserDto>>> GetAll([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<UserDto>> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var query = new GetUserByIdQuery() { Id = id };
            var eventSignal = await _mediator.Send(query, cancellationToken);
            return Ok(eventSignal);
        }
    }
}
