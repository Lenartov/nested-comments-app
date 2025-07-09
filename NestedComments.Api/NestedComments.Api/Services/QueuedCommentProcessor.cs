using NestedComments.Api.Services.Interfaces;
using NestedComments.Api.Data;

namespace NestedComments.Api.Services
{
    public class QueuedCommentProcessor : BackgroundService
    {
        private readonly ILogger<QueuedCommentProcessor> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommentQueueService _queueService;

        private const int BatchSize = 10;
        private readonly TimeSpan DelayBetweenBatches = TimeSpan.FromSeconds(1);

        public QueuedCommentProcessor(
            ILogger<QueuedCommentProcessor> logger,
            IServiceProvider serviceProvider,
            ICommentQueueService queueService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Comment queue processor started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var batch = new List<CommentQueueItem>();

                    while (batch.Count < BatchSize && _queueService.TryDequeue(out var item))
                    {
                        if (item != null)
                            batch.Add(item);
                    }

                    if (batch.Count > 0)
                    {
                        var dtos = batch.Select(i => i.Dto).ToList();

                        using var scope = _serviceProvider.CreateScope();
                        var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();

                        await commentService.CreateCommentsAsync(batch.ToArray());

                        _logger.LogInformation($"Processed batch of {batch.Count} comments.");
                    }
                    else
                    {
                        await Task.Delay(DelayBetweenBatches, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    if (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogError(ex, "Error processing comment batch.");
                        await Task.Delay(DelayBetweenBatches, stoppingToken);
                    }
                }
            }

            _logger.LogInformation("Comment queue processor stopping");
        }
    }
}
