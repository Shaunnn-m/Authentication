using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.GetUser;

public sealed record GetUserQuery(
    UserIdentifierType IdentifierType,
    string Identifier
) : IRequest<Result<GetUserResponse>>;
