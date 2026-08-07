using Authentication.Api.Application.Common;

namespace Authentication.Api.Application.Errors
{
    public static class AuthenticationErrors
    {
        public static readonly Error InvalidCredentials =
            new(
                "Authentication.InvalidCredentials",
                "The email or password is incorrect.");

        public static readonly Error InvalidToken =
            new(
                "Authentication.InvalidToken",
                "The supplied token is invalid.");

        public static readonly Error TokenExpired =
            new(
                "Authentication.TokenExpired",
                "The supplied token has expired.");
    }
}
