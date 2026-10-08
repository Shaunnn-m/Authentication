using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.UpdateMyProfile;

public sealed record UpdateMyProfileCommand(
    string FirstName,
    string LastName)
    : IRequest<Result<UpdateMyProfileResponse>>, IUnitOfWorkCommand;
