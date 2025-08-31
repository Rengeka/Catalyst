using Catalyst.Core.Structs;

namespace Catalyst.Core;

public interface IPipeline : IRunnable
{
    public static abstract IPipeline Create(string name);

    public IPipeline AddStage(string name, Action<IStage> action);
    public IPipeline AddTrigger(Action<Trigger> action);
    public IPipeline SetRunner(RunningMachine machine);
}