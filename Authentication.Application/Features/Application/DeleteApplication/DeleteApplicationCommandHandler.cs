using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Application.DeleteApplication;

public sealed class DeleteApplicationCommandHandler
    : IRequestHandler<DeleteApplicationCommand, Result<Guid>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        ICurrentUser currentUser)
    {
        _applicationRepository = applicationRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        DeleteApplicationCommand request,
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

        await _applicationRepository.DeleteAsync(
            application,
            cancellationToken);

        return Result<Guid>.Success(application.Id);
    }
}
