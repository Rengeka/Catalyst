# 🚀 Catalyst

**Catalyst** is a *platform-agnostic* solution for defining **CI/CD pipelines as code**.  
It is inspired by the *Pipeline as Code* principle and provides a clean C# API to describe workflows without being tied to a specific CI/CD provider (GitHub Actions, GitLab CI, Jenkins, etc.).

---

## ✨ Features
- Platform-agnostic — define your pipeline once, run it across multiple CI/CD providers.
- Fluent, extensible C# API for pipeline definitions.
- Support for stages, steps, triggers, and runners.
- Override default runners for custom environments.
- Designed to be backend-agnostic with planned support for multiple platforms.

---

## 📦 Installation
> ⚠️ The project is still in development.  

Install packages Catalyst.Core, Catalyst.Github (If you are running github actions) and Catalyst.CLI (Optional)
```bash
dotnet add package Catalyst.Core --version 1.0.0
dotnet add package Catalyst.Github --version 1.0.0
dotnet add package Catalyst.CLI --version 1.0.0
```

You may create a .NET project manually and include the Core and Github packages or you may use Catalyst.CLI:
```bash
catalyst init
```

Install dotnet script:
```bash
dotnet tool install -g dotnet-script
```

This will create pipeline.csx sample file. Just run:
```bash 
dotnet script pipeline.csx
```

---

## 📝 Example

```csharp
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
                "));
    })
    .AddStage("publish", stage =>
    {
        stage.WaitFor("build")
             .AddStep("Publish", step =>
             {

             });
    });

pipeline.Build();