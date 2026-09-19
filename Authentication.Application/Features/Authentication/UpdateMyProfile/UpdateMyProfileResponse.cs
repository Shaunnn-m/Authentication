namespace Authentication.Application.Features.Authentication.UpdateMyProfile;

public sealed record UpdateMyProfileResponse(
    string FirstName,
    string LastName,
    string Message);
