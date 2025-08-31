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
NuGet package publishing is planned for future releases.

For now, you can include Catalyst via a local `ProjectReference`.

---

## 📝 Example

```csharp
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

pipeline.Build();