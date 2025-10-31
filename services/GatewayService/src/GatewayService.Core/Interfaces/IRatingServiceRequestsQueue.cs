using GatewayService.Core.Models;

namespace GatewayService.Core.Interfaces;

public interface IRatingServiceRequestsQueue
{
    ValueTask EnqueueAsync(ReturnBookRequest model);
    
    ValueTask<ReturnBookRequest> DequeueAsync(CancellationToken cancellationToken);

}