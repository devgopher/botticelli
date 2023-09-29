namespace Botticelli.Server.Analytics.Utils
{
    public static class DateTimeUtils
    {
        public static IEnumerable<(DateTime dt1, DateTime dt2)> GetRange(DateTime from, DateTime to, TimeSpan period)
        {
            if (to < from) throw new InvalidDataException($"{from} > {to}!");

            var prev = from;
            var next = prev + period <= to ? prev + period : to;

            while (next - prev >= period)
            {
                var pp = prev;
                var nn = next;
                prev += period;
                next = prev + period <= to ? prev + period : to;

                yield return (pp, nn);
            }
        }
    }
}
