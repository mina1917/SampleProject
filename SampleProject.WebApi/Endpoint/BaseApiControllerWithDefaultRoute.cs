using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleProject.WebApi.Endpoint
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiControllerWithDefaultRoute : BaseApiController
    {
    }
}
