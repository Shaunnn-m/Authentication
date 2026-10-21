using Authentication.Application.Features.Application.Register;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using ApplicationEntity = Authentication.Domain.Applications.Application;
using MediatR;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using Authentication.Application.Common.Messages;
using Authentication.Domain.Applications;
using Authentication.Domain.Enums;

namespace Authentication.Application.Features.Applications.Register;

public sealed class RegisterApplicationCommandHandler
    : IRequestHandler<
        RegisterApplicationCommand,
        Result<RegisterApplicationResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ICurrentUser _currentUserService;
    private readonly IApplicationClientCredentialRepository _clientCredentialRepository;
    private readonly IClientSecretGenerator _clientSecretGenerator;

    public RegisterApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        ICurrentUser currentUserService,
        IApplicationClientCredentialRepository clientCredentialRepository,
        IClientSecretGenerator clientSecretGenerator)
    {
        _applicationRepository = applicationRepository;
        _currentUserService = currentUserService;
        _clientCredentialRepository = clientCredentialRepository;
        _clientSecretGenerator = clientSecretGenerator;
    }

    public async Task<Result<RegisterApplicationResponse>> Handle(
        RegisterApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Result<RegisterApplicationResponse>.Failure(
                UserMessages.AuthenticationRequired);
        }

        var clientId = Guid.NewGuid().ToString("N");

        var application = ApplicationEntity.Create(
            request.Name.Trim(),
            clientId,
            request.ClientType,
            userId.Value);

        await _applicationRepository.AddAsync(
            application,
            cancellationToken);

        string? clientSecret = null;

        if (request.ClientType == ApplicationClientType.Confidential)
        {
            clientSecret = _clientSecretGenerator.Generate();

            var secretHash = _clientSecretGenerator.Hash(clientSecret);

            var credential = ApplicationClientCredential.Create(
                application.Id,
                "Initial credential",
                secretHash);

            await _clientCredentialRepository.AddAsync(
                credential,
                cancellationToken);
        }

        return Result<RegisterApplicationResponse>.Success(
            new RegisterApplicationResponse(
                application.Id,
                application.Name,
                application.ClientId,
                clientSecret));
    }
}