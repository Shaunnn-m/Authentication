using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.Logout;

public sealed record LogoutCommand(
    string RefreshToken) : IRequest<Result<LogoutResponse>>, IUnitOfWorkCommand;
