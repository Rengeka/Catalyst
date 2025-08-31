using Catalyst.Core;
using Catalyst.Core.Structs;

namespace Catalyst.Github;

public class GithubPipeline : IPipeline
{
    private readonly string _name;

    private readonly List<IStage> _stages;
    private readonly List<Trigger> _triggers;

    private GithubPipeline(string name)
    {
        _name = name;
        _stages = new List<IStage>();
        _triggers = new List<Trigger>();
    }

    private RunningMachine? _runningMachine;
    public RunningMachine RunningMachine => _runningMachine ?? IRunnable.DefaultRunningMachine;

    public static IPipeline Create(string name)
    {
        return new GithubPipeline(name);
    }

    public IPipeline AddStage(string name, Action<IStage> action)
    {
        var stage = new GithubStage(name);
        action(stage);
        _stages.Add(stage);
        return this;
    }

    public IPipeline AddTrigger(Action<Trigger> action)
    {
        var trigger = new Trigger();
        action(trigger);
        _triggers.Add(trigger);
        return this;
    }

    public IPipeline SetRunner(RunningMachine machine)
    {
        _runningMachine = machine;
        machine.AddRunnable(this);
        return this;
    }
}