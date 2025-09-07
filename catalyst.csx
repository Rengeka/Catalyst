#r "nuget: Catalyst.Core, 1.0.0"
#r "nuget: Catalyst.Github, 1.0.0"

using Catalyst.Core;
using Catalyst.Core.Enums;
using Catalyst.Github;

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
                    dotnet-version: 9.x
                "))
        .AddStep("Restore dependencies", step =>
            step.SetRawAction(@"
                run: dotnet restore src/Catalyst.sln
                "))
        .AddStep("Build", step =>
            step.SetRawAction(@"
                run: dotnet build src/Catalyst.sln --configuration Release --no-restore
                "))
        //.AddStep("Run tests", step =>
        //    step.SetRawAction(@"
        //        run: dotnet test src/Catalyst.sln --configuration Release --no-build
        //        "))
        .AddStep("Pack", step =>
            step.SetRawAction(@"
                run: dotnet pack src/Catalyst.sln --configuration Release --no-build -o ./artifacts
                "))
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