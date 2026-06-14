using Microsoft.JSInterop;

namespace Botticelli.Server.FrontNew;

public class ClipboardAccessor
{
    private readonly IJSRuntime _jsRuntime;
    private Lazy<IJSObjectReference> _accessorJsRef = new();

    public ClipboardAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
            _accessorJsRef =
                new Lazy<IJSObjectReference>(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/ClipboardAccessor.js"));
    }

    public async Task CopyTextAsync(string text)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("copyText", text);
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
            await _accessorJsRef.Value.DisposeAsync();
    }
}
