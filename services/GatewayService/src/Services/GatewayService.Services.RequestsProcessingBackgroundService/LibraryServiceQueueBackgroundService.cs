using GatewayService.Clients;
using GatewayService.Core.Interfaces;
using GatewayService.Core.Models.Enums;
using GatewayService.Dto.Http.Converters.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GatewayService.Services.RequestsProcessingBackgroundService;

public class LibraryServiceQueueBackgroundService : BackgroundService
{
    private readonly ILibraryServiceRequestsQueue _libraryServiceRequestsQueue;
    private readonly IRatingServiceRequestsQueue _ratingServiceRequestsQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LibraryServiceQueueBackgroundService> _logger;  

    public LibraryServiceQueueBackgroundService(ILibraryServiceRequestsQueue libraryServiceRequestsQueue,
        IRatingServiceRequestsQueue ratingServiceRequestsQueue,
        IServiceScopeFactory scopeFactory, 
        ILogger<LibraryServiceQueueBackgroundService> logger)
    {
        _libraryServiceRequestsQueue = libraryServiceRequestsQueue;
        _ratingServiceRequestsQueue = ratingServiceRequestsQueue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var request = await _libraryServiceRequestsQueue.DequeueAsync(stoppingToken);

                if (request.ReturnBookRequestState == ReturnBookRequestState.RatingRequestFailed)
                {
                    await _ratingServiceRequestsQueue.EnqueueAsync(request);
                    continue;
                }
                
                using var scope = _scopeFactory.CreateScope();
                var libraryServiceClient = scope.ServiceProvider.GetRequiredService<ILibraryServiceClient>();

                try
                {
                    var libraryRequest = request.LibraryRequest!;
                    
                    _logger.LogDebug("Processing library request for check in book {BookId} in library {LibraryId}",
                        libraryRequest.BookId,
                        libraryRequest.LibraryId);

                    var checkInBookResponse = await libraryServiceClient.CheckInBookAsync(libraryRequest.LibraryId,
                        libraryRequest.BookId,
                        BookConditionConverter.Convert(libraryRequest.BookCondition));
                    
                    //TODO: отвратительно размазана логика, поправить.
                    if (checkInBookResponse.NewBook.Condition != checkInBookResponse.OldBook.Condition)
                        request.Penalty += 10;
                    
                    _logger.LogInformation("Successfully process library check in request");

                    await _ratingServiceRequestsQueue.EnqueueAsync(request);
                }
                catch (Exception e)
                {
                    //TODO: а если за это время данные стали не консистентны? Тут бы лимит: условно 5 ошибок для реквеста => проблема БЛ, выкидываем
                    _logger.LogError(e, "Error while processing request. {@Request}", request);
                    
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    await _libraryServiceRequestsQueue.EnqueueAsync(request);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Service is stopping");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while saving events!");
            }
        }
    }

}