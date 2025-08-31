using Catalyst.Core.Enums;
using Catalyst.Core.Structs;
using Catalyst.Github;

namespace Catalyst.CLI;

internal class TEST
{
    void RunTest()
    {
        var ubuntu = new RunningMachine("ubuntu-latest");
        var arch = new RunningMachine("archlinux");

        var pipeline = GithubPipeline.Create("name")
            .SetRunner(ubuntu)
            .AddTrigger(t =>
            {
                t.Branches = ["main", "develop"];
                t.TriggerType = TriggerType.Merge | TriggerType.PullRequest;
            })
            .AddStage("stage-1", s =>
            {
                s.SetRunner(arch)
                .AddStep(j =>
                {

                })
                .AddStep(j =>
                {

                });
            })
            .AddStage("stage-2", s =>
            {

            }); 
    }
}