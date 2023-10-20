using Microsoft.JSInterop;

namespace Botticelli.Server.FrontNew;

public class CookieStorageAccessor
{
    private readonly IJSRuntime _jsRuntime;
    private Lazy<IJSObjectReference> _accessorJsRef = new();

    public CookieStorageAccessor(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false) _accessorJsRef = new Lazy<IJSObjectReference>(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CookieStorageAccessor.js"));
    }

    public async Task<string> GetValueAsync(string key)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<string>("get", key);
        result = result.Replace($"{key}=", "")
                       .Replace(";", string.Empty)
                       .Replace("\n", string.Empty)
                       .Replace("\r", string.Empty);
        result = result.Substring(0, result.Contains(' ') ? result.IndexOf(' ') : result.Length);

        return result.Trim();
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", key, value);
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated) await _accessorJsRef.Value.DisposeAsync();
    }
}