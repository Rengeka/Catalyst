using Catalyst.Core.Enums;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Catalyst.Github.Serializers;

// TODO Split into different serializers
public sealed class GithubPipelineSerializer : IYamlTypeConverter
{
    public IValueSerializer ValueSerializer { get; set; }
    public IValueDeserializer ValueDeserializer { get; set; }

    public Dictionary<TriggerType, string>  triggerNames = new()
    {
        { TriggerType.Push, "push" },
        { TriggerType.PullRequest, "pull_request" }
    };

    public bool Accepts(Type type)
    {
        return type == typeof(GithubPipeline); 
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        var pipeline = (GithubPipeline)value;

        emitter.Emit(new MappingStart());

        emitter.Emit(new Scalar("name"));
        emitter.Emit(new Scalar(pipeline.Name));

        if (pipeline.Triggers is not null)
        {
            emitter.Emit(new Scalar("on"));
            emitter.Emit(new MappingStart()); 

            foreach (var trigger in pipeline.Triggers)
            {
                foreach (var triggerType in Enum.GetValues(typeof(TriggerType)).Cast<TriggerType>())
                {
                    if (triggerType == TriggerType.None) continue;

                    if (trigger.TriggerType.HasFlag(triggerType))
                    {
                        var triggerName = triggerNames[triggerType];

                        emitter.Emit(new Scalar(triggerName));

                        emitter.Emit(new MappingStart());
                        emitter.Emit(new Scalar("branches"));
                        emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Flow));

                        foreach (var branch in trigger.Branches)
                        {
                            emitter.Emit(new Scalar(branch));
                        }

                        emitter.Emit(new SequenceEnd());
                        emitter.Emit(new MappingEnd());
                    }
                }
            }

            emitter.Emit(new MappingEnd());
        }

        emitter.Emit(new Scalar("jobs"));
        emitter.Emit(new MappingStart());

        foreach (GithubStage stage in pipeline.Jobs)
        {
            emitter.Emit(new Scalar(stage.Name));
            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar("runs-on"));
            if (stage.RunningMachine is null)
            {
                emitter.Emit(new Scalar(pipeline.RunningMachine.Name));
            }
            else
            {
                emitter.Emit(new Scalar(stage.RunningMachine.Name));
            }

            emitter.Emit(new Scalar("steps"));
            emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Block)); 

            foreach (GithubStep step in stage.Steps)
            {
                emitter.Emit(new MappingStart());
                emitter.Emit(new Scalar("name"));
                emitter.Emit(new Scalar(step.Name));
                emitter.Emit(new Scalar("run"));
                emitter.Emit(new Scalar(step.StepAction));
                emitter.Emit(new MappingEnd());
            }
            emitter.Emit(new SequenceEnd());

            if (stage.RunningMachine != IRunnable.DefaultRunningMachine)
            {
                emitter.Emit(new Scalar("runs-on"));
                emitter.Emit(new Scalar(stage.RunningMachine.Name));
            }

            emitter.Emit(new MappingEnd()); 
        }

        emitter.Emit(new MappingEnd());

        emitter.Emit(new MappingEnd()); 
    }
}