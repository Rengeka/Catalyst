using Catalyst.Core.Structs;

namespace Catalyst.Core;

/// <summary>
/// Defines a contract for a CI/CD pipeline workflow that is platform-agnostic.
/// </summary>
public interface IPipeline : IRunnable
{
    /// <summary>
    /// Factory method for creating a new pipeline instance with the given name.
    /// </summary>
    /// <param name="name">The unique name of the pipeline.</param>
    /// <returns>A new <see cref="IPipeline"/> instance.</returns>
    public static abstract IPipeline Create(string name);

    /// <summary>
    /// Adds a new stage to the pipeline.
    /// </summary>
    /// <param name="name">The name of the stage.</param>
    /// <param name="action">
    /// A delegate that configures the stage through the <see cref="IStage"/> interface.
    /// </param>
    /// <returns>The current <see cref="IPipeline"/> instance for chaining.</returns>
    public IPipeline AddStage(string name, Action<IStage> action);

    /// <summary>
    /// Defines a trigger for the pipeline.
    /// </summary>
    /// <param name="action">
    /// A delegate that configures the trigger using a <see cref="Trigger"/> instance.
    /// </param>
    /// <returns>The current <see cref="IPipeline"/> instance for chaining.</returns>
    public IPipeline AddTrigger(Action<Trigger> action);

    /// <summary>
    /// Sets the runner machine on which the pipeline will execute.
    /// </summary>
    /// <param name="machine">The <see cref="RunningMachine"/> that will run the pipeline.</param>
    /// <returns>The current <see cref="IPipeline"/> instance for chaining.</returns>
    public IPipeline SetGlobalRunner(RunningMachine machine);

    public bool Build();
}