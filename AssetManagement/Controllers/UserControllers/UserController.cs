using Application.Commands.UserCommands.ForgotPassword;
using Application.Commands.UserCommands.Login;
using Application.Commands.UserCommands.RefreshToken;
using Application.Commands.UserCommands.Register;
using Application.Commands.UserCommands.ResendCodeToRest;
using Application.Commands.UserCommands.ResetPassword;
using Application.Commands.UserCommands.RevokeToken;
using Application.Commands.UserCommands.ValidateResetToken;
using Application.Commands.UserCommands.VerifyEmail;
using Domain.Entities.UserEntity;
using Infrastructure.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers.UserControllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerAccountCommand)
    {
        return Ok(await mediator.Send(registerAccountCommand));
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand loginCommand)
    {
        return Ok(await mediator.Send(loginCommand));
    }

    [HttpPost]
    [Route("Verify-Email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailUserCommand verifyEmailCommand)
    {
        return Ok(await mediator.Send(verifyEmailCommand));
    }

    [HttpPost]
    [Route("Refresh-Token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenUserCommand refreshTokenAccountCommand)
    {
        return Ok(await mediator.Send(refreshTokenAccountCommand));
    }
    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordUserCommand forgotPasswordAccountCommand)
    {
        return Ok(await mediator.Send(forgotPasswordAccountCommand));
    }

    [HttpPost]
    [Route("Reset-Password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordUserCommand resetPasswordAccountCommand)
    {
        return Ok(await mediator.Send(resetPasswordAccountCommand));
    }


    [HttpPost]
    [Route("Validate-Reset-Token")]
    public async Task<IActionResult> ValidateResteToken([FromBody] ValidateResetTokenUserCommand validateResetTokenAccountCommand)
    {
        return Ok(await mediator.Send(validateResetTokenAccountCommand));
    }

    [HttpPost]
    [Route("Revoke-Token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenUserCommand revokeTokenAccountCommand)
    {
        return Ok(await mediator.Send(revokeTokenAccountCommand));
    }

    [HttpPost]
    [Route("Resend-Code-Reset")]
    public async Task<IActionResult> ResendCodeReset([FromBody] ResendCodeToRestPasswordC resendCodeToRestAccount)
    {
        return Ok(await mediator.Send(resendCodeToRestAccount));
    }

}