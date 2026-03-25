using InoxServer.Application.Features.Auth.Commands.Login;
using InoxServer.Application.Features.Auth.Commands.Register;
using InoxServer.Application.Features.Auth.Commands.ResendVerifyEmail;
using InoxServer.Application.Features.Auth.Commands.VerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InoxServer.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var command = new VerifyEmailCommand { Token = token };
            await _mediator.Send(command);
            return Ok(new { message = "Email đã được xác thực thành công." });
        }

        [HttpPost("resend-verify-email")]
        public async Task<IActionResult> ResendVerifyEmail([FromBody] ResendVerifyEmailCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Email xác thực đã được gửi lại. Vui lòng kiểm tra hộp thư của bạn." });
        }
    }
}
