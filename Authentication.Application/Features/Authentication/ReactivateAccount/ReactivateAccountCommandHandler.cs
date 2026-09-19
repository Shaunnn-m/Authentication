using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.ReactivateAccount;

public sealed class ReactivateAccountCommandHandler
    : IRequestHandler<ReactivateAccountCommand, Result<ReactivateAccountResponse>>
{
    private readonly IUserService _userService;

    public ReactivateAccountCommandHandler(
        IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result<ReactivateAccountResponse>> Handle(
        ReactivateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.ReactivateAccountAsync(
            request.Email,
            cancellationToken);

        if (!result.IsSuccess)
        {
            return Result<ReactivateAccountResponse>.Failure(result.Error!);
        }

        return Result<ReactivateAccountResponse>.Success(
            new ReactivateAccountResponse("If an eligible account exists, a reactivation link has been sent."));
    }
}
