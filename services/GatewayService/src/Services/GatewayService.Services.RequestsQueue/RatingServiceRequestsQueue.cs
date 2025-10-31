using System.Threading.Channels;
using GatewayService.Core.Interfaces;
using GatewayService.Core.Models;

namespace GatewayService.Services.RequestsQueue;

public class RatingServiceRequestsQueue : IRatingServiceRequestsQueue
{
    private readonly Channel<ReturnBookRequest> _queue;

    public RatingServiceRequestsQueue()
    {
        //TODO: moved to config
        var options = new BoundedChannelOptions(10000)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<ReturnBookRequest>(options);
    }

    public async ValueTask EnqueueAsync(ReturnBookRequest model)
    {
        await _queue.Writer.WriteAsync(model);
    }

    async ValueTask<ReturnBookRequest> IRatingServiceRequestsQueue.DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}