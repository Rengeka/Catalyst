using Catalyst.Core;

namespace Catalyst.Github;

public class GithubStage : IStage
{
    public string Name { get; set; }

    public List<IStep> Steps { get; init; }

    public GithubStage(string name)
    {
        Name = name;
        Steps = new List<IStep>();
    }

    private RunningMachine? _runningMachine;

    public RunningMachine RunningMachine => _runningMachine ?? IRunnable.DefaultRunningMachine;

    public IStage AddStep(string name, Action<IStep> action)
    {
        var step = new GithubStep(name);
        action(step);
        Steps.Add(step);
        return this;
    }

    public IStage AddStep(string name, string stepAction)
    {
        var step = new GithubStep(name, stepAction);
        Steps.Add(step);
        return this;
    }

    public IStage SetRunner(RunningMachine runningMachine)
    {
        _runningMachine = runningMachine;
        return this;
    }
}