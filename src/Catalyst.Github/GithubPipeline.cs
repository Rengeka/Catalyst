using Catalyst.Core;
using Catalyst.Core.Structs;
using Catalyst.Github.Serializers;
using YamlDotNet.Serialization;

namespace Catalyst.Github;

public class GithubPipeline : IPipeline
{
    public string Name { get; set; }

    public List<Trigger> Triggers { get; init; }

    public string RunningMachineValue => RunningMachine.Name;

    public List<IStage> Jobs { get; init; }

    private GithubPipeline(string name)
    {
        Name = name;
        Jobs = new List<IStage>();
        Triggers = new List<Trigger>();
    }

    private RunningMachine? _runningMachine;

    [YamlIgnore]
    public RunningMachine RunningMachine => _runningMachine ?? IRunnable.DefaultRunningMachine;

    public static IPipeline Create(string name)
    {
        return new GithubPipeline(name);
    }

    public IPipeline AddStage(string name, Action<IStage> action)
    {
        var job = new GithubStage(name);
        action(job);
        Jobs.Add(job);
        return this;
    }

    public IPipeline AddTrigger(Action<Trigger> action)
    {
        var trigger = new Trigger();
        action(trigger);
        Triggers.Add(trigger);
        return this;
    }

    public IPipeline SetGlobalRunner(RunningMachine runningMachine)
    {
        _runningMachine = runningMachine;
        return this;
    }

    public bool Build()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(new KebabCaseNamingConvention())
            .WithTypeConverter(new GithubPipelineSerializer())
            //.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .Build();

        string yaml = serializer.Serialize(this);

        Directory.CreateDirectory(".github/workflows");
        File.WriteAllText(".github/workflows/ci.yml", yaml);

        return true;
    }
}