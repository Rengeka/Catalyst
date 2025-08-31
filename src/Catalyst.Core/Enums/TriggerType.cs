namespace Catalyst.Core.Enums;

[Flags]
public enum TriggerType
{
    None = 0,
    Manual = 1,
    Merge = 2,
    Commit = 4,
    PullRequest = 8,
    Push = 16,
}