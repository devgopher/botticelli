using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Botticelli.Shared.Utils;
using Flurl;

namespace Botticelli.Framework.Vk.API.Utils;

public static class ApiUtils
{
    public static Uri GetMethodUri(string baseAddress,
                                   string method,
                                   object methodParams = default,
                                   bool snakeCase = true)
    {
        if (methodParams == default) 
            return new Uri(Url.Combine(baseAddress, "method", method));

        if (!snakeCase) 
            return new Uri(Url.Combine(baseAddress, "method", method).SetQueryParams(methodParams));

        var snaked = new Dictionary<string, object>();
        var props = methodParams.GetType().GetProperties();


        foreach (var prop in props)
        {
            var value = prop.GetValue(methodParams) ?? string.Empty;

            snaked[prop.Name.ToSnakeCase()] = value;
        }

        return new Uri(Url.Combine(baseAddress, "method", method).SetQueryParams(snaked));
    }

    public static MultipartFormDataContent GetMethodMultipartFormContent(object methodParams = default,
                                                                        bool snakeCase = true)
    {
        var props = methodParams.GetType().GetProperties();
        var content = new MultipartFormDataContent();

        foreach (var prop in props)
        {
            var value = prop.GetValue(methodParams) as string ?? string.Empty;

            content.Add(new StringContent(value), snakeCase ? prop.Name.ToSnakeCase() : prop.Name);
        }

        return content;
    }

    public static Uri GetMethodUriWithJson(string baseAddress, string method, object methodParams = default)
    {
        var snaked = new Dictionary<string, object>();
        var props = methodParams.GetType().GetProperties();


        foreach (var prop in props)
        {
            var value = prop.GetValue(methodParams) ?? string.Empty;

            var jpName = prop.GetCustomAttribute<JsonPropertyNameAttribute>();

            snaked[jpName?.Name ?? prop.Name] = value;
        }

        return new Uri(Url.Combine(baseAddress, "method", method).SetQueryParams(snaked));
    }
}