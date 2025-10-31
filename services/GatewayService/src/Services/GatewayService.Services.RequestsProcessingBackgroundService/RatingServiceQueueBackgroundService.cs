using GatewayService.Clients;
using GatewayService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RatingService.Dto.Http;

namespace GatewayService.Services.RequestsProcessingBackgroundService;

public class RatingServiceQueueBackgroundService : BackgroundService
{
    private readonly IRatingServiceRequestsQueue _ratingServiceRequestsQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LibraryServiceQueueBackgroundService> _logger;  

    public RatingServiceQueueBackgroundService(IRatingServiceRequestsQueue ratingServiceRequestsQueue,
        IServiceScopeFactory scopeFactory, 
        ILogger<LibraryServiceQueueBackgroundService> logger)
    {
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
                var request = await _ratingServiceRequestsQueue.DequeueAsync(stoppingToken);
                
                using var scope = _scopeFactory.CreateScope();
                var ratingServiceClient = scope.ServiceProvider.GetRequiredService<IRatingServiceClient>();

                try
                {
                    _logger.LogDebug("Processing rating request for user {UserName}", request.RatingRequest.UserName);
                    
                    var ratingRequest = request.RatingRequest;

                    var rating = await ratingServiceClient.GetRatingAsync(ratingRequest.UserName);
            
                    var newCountStars = request.Penalty == 0 ? rating.Stars + 1 : rating.Stars - request.Penalty;
            
                    await ratingServiceClient.UpdateRatingAsync(ratingRequest.UserName, new UpdateRatingRequest(newCountStars));
                    
                    _logger.LogInformation("Rating {UserName} is updated.", ratingRequest.UserName);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while processing request. {@Request}", request);
                    
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    await _ratingServiceRequestsQueue.EnqueueAsync(request);
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