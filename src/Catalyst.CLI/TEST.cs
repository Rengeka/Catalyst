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
                t.Branches = new[] { "main" };
                t.TriggerType = TriggerType.PullRequest;
            })
            .AddTrigger(t =>
            {
                t.Branches = new[] { "main" };
                t.TriggerType = TriggerType.Push;
            })
            .AddStage("build", stage =>
            {
                stage.AddStep("Checkout", step =>
                        step.SetRawAction("uses", "actions/checkout@v3"))
                     .AddStep("Install .NET SDK", step =>
                        step.SetRawAction("uses", "actions/setup-dotnet@v3"))
                     .AddStep("Check Dotnet Version", "dotnet --version");
            })
            .AddStage("test", stage =>
            {
                stage.AddStep("Checkout", step =>
                        step.SetRawAction("uses", "actions/checkout@v3"))
                     .AddStep("Install .NET SDK", step =>
                        step.SetRawAction("uses", "actions/setup-dotnet@v3"))
                     .AddStep("Run tests", "dotnet test --configuration Release")
                     .WaitFor("build");
            })
            .AddStage("deploy", stage =>
            {
                stage.AddStep("Checkout", step =>
                        step.SetRawAction("uses", "actions/checkout@v3"))
                     .AddStep("Deploy", "deploy-scripts/deploy.sh")
                     .WaitFor("test")
                     .If("github.ref == 'refs/heads/main' && github.event_name == 'push'");
            });

        pipeline.Build();
    }
}