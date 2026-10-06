using Authentication.Application.Common.Results.Applications;
using Authentication.Application.Features.Application.Register;
using Authentication.Application.Interfaces.Applications;
using ApplicationEntity = Authentication.Domain.Applications.Application;
using MediatR;

namespace Authentication.Application.Features.Applications.Register;

public sealed class RegisterApplicationCommandHandler
    : IRequestHandler<
        RegisterApplicationCommand,
        RegisterApplicationResult>
{
    private readonly IApplicationRepository _applicationRepository;

    public RegisterApplicationCommandHandler(
        IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<RegisterApplicationResult> Handle(
        RegisterApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var clientId = Guid.NewGuid().ToString("N");

        var application = ApplicationEntity.Create(
            request.Name,
            clientId);

        await _applicationRepository.AddAsync(
            application,
            cancellationToken);

        return new RegisterApplicationResult(
            application.Id,
            application.Name,
            application.ClientId);
    }
}