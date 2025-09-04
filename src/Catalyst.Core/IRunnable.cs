using Catalyst.Core;

/// <summary>
/// Represents an entity that can be executed on a specific
/// <see cref="Catalyst.Core.RunningMachine"/>.
/// Provides a default runner for convenience.
/// </summary>
public interface IRunnable
{
    /// <summary>
    /// The name of the default <see cref="Catalyst.Core.RunningMachine"/> 
    /// used when no explicit runner is provided.
    /// Typically corresponds to a commonly available build environment.
    /// </summary>
    private const string DEFAULT_RUNNING_MACHINE_NAME = "ubuntu-latest";

    private static readonly RunningMachine _defaultRunningMachine = new(DEFAULT_RUNNING_MACHINE_NAME);

    /// <summary>
    /// The default <see cref="Catalyst.Core.RunningMachine"/> instance
    /// used when no specific runner is provided.
    /// Typically points to a common build environment (e.g., "ubuntu-latest").
    /// </summary>
    public static RunningMachine DefaultRunningMachine => _defaultRunningMachine;

    /// <summary>
    /// Gets the <see cref="Catalyst.Core.RunningMachine"/> 
    /// on which this runnable will execute.
    /// </summary>
    public RunningMachine RunningMachine { get; }
}