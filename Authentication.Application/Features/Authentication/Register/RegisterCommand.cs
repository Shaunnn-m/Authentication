using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.Register;

public sealed record RegisterCommand(
    Guid ApplicationId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<Result<RegisterResponse>>, IUnitOfWorkCommand;