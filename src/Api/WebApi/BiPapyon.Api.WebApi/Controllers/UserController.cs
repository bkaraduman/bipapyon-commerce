using BiPapyon.Common.Models;
using BiPapyon.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BiPapyon.Api.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(new ApiResponseDTO { Result = res });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(new ApiResponseDTO { Result = res });
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(new ApiResponseDTO { Result = res });
        }
    }
}