using Authentication.Application.Interfaces.Identity;
using Authentication.Application.Common.Results;
using MediatR;
using Authentication.Application.Common.Messages;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;

namespace Authentication.Application.Features.Authentication.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserService _userService;
    private readonly IEmailConfirmationService _emailConfirmationService;

    public RegisterCommandHandler(IUserService userService, IEmailConfirmationService emailConfirmationService)
    {
        _userService = userService;
        _emailConfirmationService = emailConfirmationService;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _userService.ExistsByEmailAsync(
            request.Email,
            cancellationToken);

        if (exists)
        {
            return Result<RegisterResponse>.Failure(
                UserMessages.AlreadyExists);
        }

        var result = await _userService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<RegisterResponse>.Failure(
                UserMessages.AlreadyExists);
        }

        var confirmationResult =
            await _emailConfirmationService.HandleAsync(
                result.Value,
                request.FirstName,
                request.Email,
                cancellationToken);

        if (confirmationResult.IsFailure)
        {
            return Result<RegisterResponse>.Failure(
                confirmationResult.Error!);
        }


        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                result.Value,
                request.Email));
    }
}