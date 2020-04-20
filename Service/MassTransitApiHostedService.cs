using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitStudies.Service
{
    public class MassTransitApiHostedService :
        IHostedService
    {
        readonly IBusControl _bus;
        private ILogger<MassTransitApiHostedService> _logger;

        public MassTransitApiHostedService(IBusControl bus, ILoggerFactory loggerFactory)
        {
            _bus = bus;
            _logger = loggerFactory.CreateLogger<MassTransitApiHostedService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting host");
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping host");
            await _bus.StopAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
