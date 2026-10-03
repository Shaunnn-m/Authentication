using Authentication.Application.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Authentication.Application.Common.Behaviors;

public sealed class OperationLoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<OperationLoggingBehavior<TRequest, TResponse>> _logger;

    public OperationLoggingBehavior(
        ILogger<OperationLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var operationName = typeof(TRequest).Name;

        try
        {
            var response = await next();
            stopwatch.Stop();

            if (response is Result { IsFailure: true })
            {
                _logger.LogInformation(
                    "Application operation {OperationName} completed with a failure in {ElapsedMilliseconds} ms.",
                    operationName,
                    stopwatch.ElapsedMilliseconds);
            }
            else if (operationName.EndsWith("Command", StringComparison.Ordinal))
            {
                _logger.LogInformation(
                    "Application operation {OperationName} completed in {ElapsedMilliseconds} ms.",
                    operationName,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogDebug(
                    "Application operation {OperationName} completed in {ElapsedMilliseconds} ms.",
                    operationName,
                    stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogDebug(
                "Application operation {OperationName} was cancelled after {ElapsedMilliseconds} ms.",
                operationName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Application operation {OperationName} failed after {ElapsedMilliseconds} ms.",
                operationName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}