using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.GetMyAccount;

public sealed record GetMyAccountQuery()
    : IRequest<Result<GetMyAccountResponse>>;
