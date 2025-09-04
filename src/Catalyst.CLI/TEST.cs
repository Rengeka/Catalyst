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
            .AddStage("build", stage =>
            {
                stage.AddStep("checkout", step =>
                    step.SetAction("git checkout $(BranchName)"))
                     .AddStep("build", "dotnet build --no-restore --configuration Release");
            })
            .AddStage("test", stage =>
            {
                stage.AddStep("test", "dotnet test --configuration Release")
                     .WaitFor("Build"); 
            })
            .AddStage("deploy", stage =>
            {
                stage.AddStep("deploy", "deploy-scripts/deploy.sh")
                     .WaitFor("Test")
                     .If("github.ref == 'refs/heads/main' && github.event_name == 'push'");
            });

        pipeline.Build();

    }
}