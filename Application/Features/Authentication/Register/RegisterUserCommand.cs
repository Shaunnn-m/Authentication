using MediatR;
using Authentication.Api.Application.Common;

namespace Authentication.Api.Application.Features.Authentication.Register;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<Result<RegisterUserResponse>>;