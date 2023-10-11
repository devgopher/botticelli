using MediatR;

namespace Botticelli.Client.Analytics.Requests
{
    public interface IMetricRequest : IRequest
    {
        public string MetricName { get; set; }
    }
}
