namespace Botticelli.Server.Analytics.Exceptions
{
    public class AnalyticsException : Exception
    {
        public AnalyticsException(string message, Exception inner) : base(message, inner) { }
    }
}
