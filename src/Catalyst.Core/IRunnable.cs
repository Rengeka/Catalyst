namespace Catalyst.Core;

/// <summary>
/// Represents an entity that can be executed on a specific <see cref="RunningMachine"/>.
/// Provides a default runner for convenience.
/// </summary>
public interface IRunnable
{
    /// <summary>
    /// The name of the default <see cref="RunningMachine"/> used when no explicit runner is provided.
    /// Typically corresponds to a commonly available build environment.
    /// </summary>
    private const string DEFAULT_RUNNING_MACHINE_NAME = "ubuntu-latest";

    /// <summary>
    /// The default <see cref="RunningMachine"/> used when no specific runner is provided.
    /// Typically points to a common build environment (e.g., "ubuntu-latest").
    /// </summary>
    public static readonly RunningMachine DefaultRunningMachine = new(DEFAULT_RUNNING_MACHINE_NAME);

    /// <summary>
    /// Gets the <see cref="RunningMachine"/> on which this runnable will execute.
    /// </summary>
    public RunningMachine RunningMachine { get; }
}