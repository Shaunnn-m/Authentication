using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.DeactivateAccount;

public sealed record DeactivateAccountCommand()
    : IRequest<Result<DeactivateAccountResponse>>;
