using Flurl;

namespace Botticelli.Framework.Vk.API.Utils
{
    public static class ApiUtils
    {
        public static Uri GetMethodUri(string baseAddress, string method, params object[] methodParams)
            => new(Flurl.Url.Combine(baseAddress, "method", method)
                        .SetQueryParams(methodParams));

    }
}
