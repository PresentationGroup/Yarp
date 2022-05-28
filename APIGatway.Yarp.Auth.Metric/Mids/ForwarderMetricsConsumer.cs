using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.Telemetry.Consumption;

namespace APIGatway.Yarp.Auth.Metric.Mids
{
    public sealed class ForwarderMetricsConsumer : IMetricsConsumer<ForwarderMetrics>
    {
        public void OnMetrics(ForwarderMetrics previous, ForwarderMetrics current)
        {
            var elapsed = current.Timestamp - previous.Timestamp;
            var newRequests = current.RequestsStarted - previous.RequestsStarted;
            Console.Title = $"Forwarded {current.RequestsStarted} requests ({newRequests} in the last {(int)elapsed.TotalMilliseconds} ms)";
        }
    }
}
