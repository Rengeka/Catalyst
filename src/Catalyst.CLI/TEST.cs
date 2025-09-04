using Catalyst.Core;
using Catalyst.Core.Enums;
using Catalyst.Github;

namespace Catalyst.CLI;

internal class TEST
{
    public static void RunTest()
    {
        var ubuntu = new RunningMachine("ubuntu-latest");

        var pipeline = GithubPipeline.Create("CI Pipeline")
            .SetGlobalRunner(ubuntu)
            .AddTrigger(t =>
            {
                t.Branches = new[] { "main", "develop" };
                t.TriggerType = TriggerType.Push | TriggerType.PullRequest;
            })
            .AddStage("Build", stage =>
            {
                stage.AddStep("checkout", step =>
                     {
                         step.SetAction("git checkout $(BranchName)");
                     })
                     .AddStep("build", "dotnet build --no-restore --configuration Release");
            })
            .AddStage("Test", stage =>
            {
                stage.AddStep("test", "dotnet test --configuration Release");
            })
            .AddStage("Deploy", stage =>
            {
                stage.AddStep("deploy", "deploy-scripts/deploy.sh");
            });

        pipeline.Build();
    }
}