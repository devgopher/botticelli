using Botticelli.Server.FrontNew;
using FluentAssertions;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using Moq;
using NUnit.Framework;

namespace Botticelli.Server.FrontNew.Tests.Accessors;

[TestFixture]
public class JsAccessorsTests
{
    [Test]
    public async Task ClipboardAccessor_Should_ImportAndCopy()
    {
        var module = new Mock<IJSObjectReference>();
        module.Setup(m => m.InvokeAsync<IJSVoidResult>("copyText", It.IsAny<object?[]?>()))
              .ReturnsAsync(Mock.Of<IJSVoidResult>());
        module.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var js = new Mock<IJSRuntime>();
        js.Setup(j => j.InvokeAsync<IJSObjectReference>("import", It.IsAny<object?[]?>()))
          .ReturnsAsync(module.Object);

        var accessor = new ClipboardAccessor(js.Object);
        await accessor.CopyTextAsync("hello");
        await accessor.DisposeAsync();

        module.Verify(m => m.InvokeAsync<IJSVoidResult>("copyText", It.Is<object?[]?>(a =>
            a != null && a.Length == 1 && (string)a[0]! == "hello")), Times.Once);
        module.Verify(m => m.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task CookieStorageAccessor_Should_GetAndSet()
    {
        var module = new Mock<IJSObjectReference>();
        module.Setup(m => m.InvokeAsync<string>("get", It.IsAny<object?[]?>()))
              .ReturnsAsync("token=abc123; path=/");
        module.Setup(m => m.InvokeAsync<IJSVoidResult>("set", It.IsAny<object?[]?>()))
              .ReturnsAsync(Mock.Of<IJSVoidResult>());
        module.Setup(m => m.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var js = new Mock<IJSRuntime>();
        js.Setup(j => j.InvokeAsync<IJSObjectReference>("import", It.IsAny<object?[]?>()))
          .ReturnsAsync(module.Object);

        var accessor = new CookieStorageAccessor(js.Object);
        var value = await accessor.GetValueAsync("token");
        await accessor.SetValueAsync("token", "xyz");
        await accessor.DisposeAsync();

        value.Should().Be("abc123");
        module.Verify(m => m.InvokeAsync<IJSVoidResult>("set", It.Is<object?[]?>(a =>
            a != null && a.Length == 2 && (string)a[0]! == "token" && (string)a[1]! == "xyz")), Times.Once);
    }
}
