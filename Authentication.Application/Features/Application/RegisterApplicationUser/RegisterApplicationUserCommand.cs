using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.RegisterApplicationUser;

public sealed record RegisterApplicationUserCommand(
    Guid ApplicationId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword)
    : IRequest<Result<RegisterApplicationUserResponse>>, IUnitOfWorkCommand;
