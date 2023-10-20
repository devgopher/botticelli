namespace Botticelli.Analytics.Shared.Responses;

public class GetMetricsResponse
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal Count { get; set; }
}