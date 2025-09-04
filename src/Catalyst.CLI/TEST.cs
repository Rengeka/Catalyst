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
                t.Branches = ["main"];
                t.TriggerType = TriggerType.PullRequest;
            })
            .AddTrigger(t =>
            {
                t.Branches = ["main"];
                t.TriggerType = TriggerType.Push;
            })
            .AddStage("build", stage =>
            {
                stage
                .AddStep("Checkout", step =>
                    step.SetRawAction(@"
                        uses: actions/checkout@v4
                        "))
                .AddStep("Setup .NET", step =>
                    step.SetRawAction(@"
                        uses: actions/setup-dotnet@v4
                        with:
                            dotnet-version: 8.x
                        "))
                .AddStep("Restore dependencies", "dotnet restore")
                .AddStep("Build", "dotnet build --configuration Release --no-restore")
                //.AddStep("Run tests", "dotnet test --configuration Release --no-build")
                .AddStep("Pack", "dotnet pack --configuration Release --no-build -o ./artifacts")
                .AddStep("Upload artifacts", step =>
                    step.SetRawAction(@"
                        uses: actions/upload-artifact@v4
                        with:
                            name: nuget-packages
                            path: ./artifacts/*.nupkg
                        "));
            })
            .AddStage("publish", stage =>
            {
                stage.WaitFor("build")
                     .If("github.ref == 'refs/heads/main' && github.event_name == 'push'")
                     .AddStep("Download artifacts", step =>
                        step.SetRawAction(@"
                            uses: actions/download-artifact@v4
                            with:
                              name: nuget-packages
                              path: ./artifacts
                            "))
                     .AddStep("Publish to NuGet", step =>
                        step.SetRawAction(@"
                            run: dotnet nuget push ./artifacts/*.nupkg --api-key ${{ secrets.NUGET_API_KEY }} --source https://api.nuget.org/v3/index.json
                            "));
            });

        pipeline.Build();
    }
}