using Catalyst.Core.Structs;

namespace Catalyst.Core;

public interface IStage : IRunnable
{
    public IStage AddStep(Action<IStep> action);
    public IStage SetRunner(RunningMachine machine);
}