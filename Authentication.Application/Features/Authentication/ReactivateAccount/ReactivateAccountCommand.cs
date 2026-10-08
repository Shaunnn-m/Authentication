using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.ReactivateAccount;

public sealed record ReactivateAccountCommand(
    string Email
)
    : IRequest<Result<ReactivateAccountResponse>>, IUnitOfWorkCommand;
