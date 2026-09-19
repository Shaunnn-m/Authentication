using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.UpdateMyProfile;

public sealed record UpdateMyProfileCommand(
    string FirstName,
    string LastName)
    : IRequest<Result<UpdateMyProfileResponse>>;
