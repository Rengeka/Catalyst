namespace Catalyst.Core;

/// <summary>
/// Represents a stage within a CI/CD <see cref="IPipeline"/>.
/// A stage is a logical grouping of steps that execute on a specified <see cref="RunningMachine"/>.
/// </summary>
public interface IStage : IRunnable
{
    /// <summary>
    /// Adds a new step to this stage.
    /// </summary>
    /// <param name="action">
    /// A delegate that configures the step through the <see cref="IStep"/> interface.
    /// </param>
    /// <returns>The current <see cref="IStage"/> instance for chaining.</returns>
    public IStage AddStep(string name, Action<IStep> action); // TODO Change docs for this class

    public IStage AddStep(string name, string stepAction);

    /// <summary>
    /// Sets the runner machine on which this stage will execute.
    /// Overrides the default runner if one is already set.
    /// </summary>
    /// <param name="machine">The <see cref="RunningMachine"/> that will run this stage.</param>
    /// <returns>The current <see cref="IStage"/> instance for chaining.</returns>
    public IStage SetRunner(RunningMachine machine);
}