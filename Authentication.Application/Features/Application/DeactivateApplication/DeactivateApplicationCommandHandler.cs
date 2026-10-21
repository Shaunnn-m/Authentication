using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Application.DeactivateApplication;

public sealed class DeactivateApplicationCommandHandler
    : IRequestHandler<DeactivateApplicationCommand, Result<Guid>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ICurrentUser _currentUser;

    public DeactivateApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        ICurrentUser currentUser)
    {
        _applicationRepository = applicationRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        DeactivateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
            request.ApplicationId,
            cancellationToken);

        if (application is null)
        {
            return Result<Guid>.Failure(ApplicationMessages.NotFound);
        }

        if (_currentUser.UserId != application.CreatedByUserId)
        {
            return Result<Guid>.Failure(ApplicationMessages.CreatorOnly);
        }

        if (!application.IsActive)
        {
            return Result<Guid>.Success(application.Id);
        }

        application.Deactivate();

        return Result<Guid>.Success(application.Id);
    }
}
