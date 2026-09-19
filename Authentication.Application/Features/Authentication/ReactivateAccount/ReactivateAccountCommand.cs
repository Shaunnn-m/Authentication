using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ReactivateAccount;

public sealed record ReactivateAccountCommand(
    string Email
)
    : IRequest<Result<ReactivateAccountResponse>>;
