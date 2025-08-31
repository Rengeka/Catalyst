namespace Catalyst.Core.Structs;

public class RunningMachine
{
    private readonly string _name;

    private readonly List<IRunnable> _runnables;
    public IReadOnlyList<IRunnable> Runnables => _runnables;


    public RunningMachine(string name)
    {
        _name = name;
        _runnables = new List<IRunnable>();
    }

    public RunningMachine AddRunnable(IRunnable runnable)
    {
        _runnables.Add(runnable);
        return this;
    }
}