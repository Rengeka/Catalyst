using Catalyst.Core.Structs;

namespace Catalyst.Core;

/// <summary>
/// Marker interface for those stages and jobs that have to executed on a concrete machine
/// </summary>
public interface IRunnable
{
    public static readonly RunningMachine DefaultRunningMachine = new("ubuntu-latest");
    public RunningMachine RunningMachine { get; }
}