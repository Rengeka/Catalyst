using Catalyst.Core;

namespace Catalyst.Github;

public class GithubStep : IStep
{
    public string Name { get; set; }
    public string StepAction { get; set; }
    public bool IsRaw { get; set; }

    public GithubStep(string name)
    {
        Name = name;
    }

    public GithubStep(string name, string stepAction)
    {
        Name = name;
        StepAction = stepAction;
    }

    public IStep SetAction(string stepAction)
    {
        StepAction = stepAction;
        IsRaw = false;
        return this;
    }

    public IStep SetRawAction(string rawStepAction)
    {
        StepAction = rawStepAction;
        IsRaw = true;
        return this;
    }
}