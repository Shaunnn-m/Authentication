using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.Register;

public sealed record RegisterCommand(
    Guid ApplicationId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<Result<RegisterResponse>>;