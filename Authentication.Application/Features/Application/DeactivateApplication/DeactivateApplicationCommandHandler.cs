using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using MediatR;

namespace Authentication.Application.Features.Application.DeactivateApplication;

public sealed class DeactivateApplicationCommandHandler
    : IRequestHandler<DeactivateApplicationCommand, Result<Guid>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ICurrentApplicationService _currentApplicationService;

    public DeactivateApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        ICurrentApplicationService currentApplicationService)
    {
        _applicationRepository = applicationRepository;
        _currentApplicationService = currentApplicationService;
    }

    public async Task<Result<Guid>> Handle(
        DeactivateApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var applicationId = _currentApplicationService.ApplicationId;

        if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
        {
            return Result<Guid>.Failure(ApplicationMessages.ContextRequired);
        }

        var application = await _applicationRepository.GetByAppplicationId(
            applicationId.Value,
            cancellationToken);

        if (application is null)
        {
            return Result<Guid>.Failure(ApplicationMessages.NotFound);
        }

        if (!application.IsActive)
        {
            return Result<Guid>.Success(application.Id);
        }

        application.Deactivate();

        return Result<Guid>.Success(application.Id);
    }
}
