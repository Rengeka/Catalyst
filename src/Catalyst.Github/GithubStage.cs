using Catalyst.Core;
using Catalyst.Core.Structs;

namespace Catalyst.Github;

public class GithubStage : IStage
{
    private readonly string _name;

    private readonly List<IStep> _step;

    public GithubStage(string name)
    {
        _name = name;
        _step = new List<IStep>();
    }

    private RunningMachine? _runningMachine;
    public RunningMachine RunningMachine => _runningMachine ?? IRunnable.DefaultRunningMachine;

    public IStage AddStep(Action<IStep> action)
    {
        var step = new GithubJob();
        action(step);

        return this;
    }

    public IStage SetRunner(RunningMachine machine)
    {
        machine.AddRunnable(this);
        return this;
    }
}