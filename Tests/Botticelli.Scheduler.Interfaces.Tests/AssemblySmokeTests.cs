using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Botticelli.Scheduler.Interfaces.Tests;

[TestFixture]
public class AssemblySmokeTests
{
    [Test]
    public void SchedulerInterfacesAssembly_Should_Load_EvenWithoutOwnTypes()
    {
        // Project currently has no source files; contracts live in Botticelli.Interfaces.
        var asm = Assembly.Load("Botticelli.Scheduler.Interfaces");
        asm.Should().NotBeNull();
        asm.GetName().Name.Should().Be("Botticelli.Scheduler.Interfaces");
    }
}
