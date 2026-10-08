using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.DeactivateAccount;

public sealed record DeactivateAccountCommand()
    : IRequest<Result<DeactivateAccountResponse>>, IUnitOfWorkCommand;
