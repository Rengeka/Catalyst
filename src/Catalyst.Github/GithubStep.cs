using Catalyst.Core;
using YamlDotNet.Serialization;

namespace Catalyst.Github;

public class GithubStep : IStep
{
    public string Name { get; set; }
    public string StepAction { get; set; }

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
        return this;
    }
}