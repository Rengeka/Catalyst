using Catalyst.Core;
using Catalyst.Core.Enums;
using Catalyst.Github;

namespace Catalyst.CLI;

internal class TEST
{
    public static void RunTest()
    {
        var ubuntu = new RunningMachine("ubuntu-latest");
        var arch = new RunningMachine("archlinux");

        var pipeline = GithubPipeline.Create("CI Pipeline")
            .SetRunner(ubuntu)
            .AddTrigger(t =>
            {
                t.Branches = new[] { "main", "develop" };
                t.TriggerType = TriggerType.Push | TriggerType.PullRequest;
            })
            .AddStage("Build and Test", stage =>
            {
                stage.SetRunner(arch)
                     .AddStep("Checkout", step =>
                     {
                         step.SetAction("git checkout $(BranchName)");
                     })
                     .AddStep("Build", "dotnet build --no-restore --configuration Release")
                     .AddStep("Test", "dotnet test --configuration Release");
            })
            .AddStage("Deploy", stage =>
            {
                stage.AddStep("Deploy", "deploy-scripts/deploy.sh");
            });

        pipeline.Build();
    }
}