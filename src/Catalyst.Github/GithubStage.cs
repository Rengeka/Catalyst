using Catalyst.Core;

namespace Catalyst.Github;

public class GithubStage : IStage
{
    public string Name { get; set; }

    public List<string> Needs { get; set; }
    public List<IStep> Steps { get; init; }
    public string Condition;

    public GithubStage(string name)
    {
        Name = name;
        Steps = new List<IStep>();
        Needs = new List<string>();
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

    public IStage WaitFor(string name)
    {
        Needs.Add(name);
        return this;
    }

    public IStage If(string condition)
    {
        Condition = condition;
        return this;
    }
}