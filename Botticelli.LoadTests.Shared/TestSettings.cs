using System.Collections.Immutable;

namespace Botticelli.LoadTests.Shared;

/// <summary>
/// Test settings
/// </summary>
public class TestSettings
{
    /// <summary>
    /// Test duration
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.MaxValue;

    /// <summary>
    /// Single test run timeout
    /// </summary>
    public TimeSpan TestRunTimeout { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Single test step run timeout
    /// </summary>
    public TimeSpan TestStepRunTimeout { get; set; } = TimeSpan.FromMinutes(1);
    
    
    /// <summary>
    /// Additional specific settings
    /// </summary>
    public ImmutableSortedDictionary<string, object> AdditionalSettings { get; set; }
}