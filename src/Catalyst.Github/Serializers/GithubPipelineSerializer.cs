using Catalyst.Core.Enums;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Catalyst.Github.Serializers;

public sealed class GithubPipelineSerializer : IYamlTypeConverter
{
    public IValueSerializer ValueSerializer { get; set; }
    public IValueDeserializer ValueDeserializer { get; set; }

    public Dictionary<TriggerType, string> triggerNames = new()
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

            string runsOnValue = stage.RunningMachine?.Name ?? pipeline.RunningMachine.Name;
            emitter.Emit(new Scalar("runs-on"));
            emitter.Emit(new Scalar(runsOnValue));

            if (stage.Needs.Any())
            {
                emitter.Emit(new Scalar("needs"));
                emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Flow));
                foreach (var job in stage.Needs)
                {
                    emitter.Emit(new Scalar(job));
                }
                emitter.Emit(new SequenceEnd());
            }

            if (stage.Condition is not null)
            {
                emitter.Emit(new Scalar("if"));
                emitter.Emit(new Scalar(stage.Condition));
            }

            emitter.Emit(new Scalar("steps"));
            emitter.Emit(new SequenceStart(null, null, true, SequenceStyle.Block));

            foreach (GithubStep step in stage.Steps)
            {
                emitter.Emit(new MappingStart());
                emitter.Emit(new Scalar("name"));
                emitter.Emit(new Scalar(step.Name));

                if (step.IsRaw)
                {
                    try
                    {
                        using (var reader = new StringReader(step.StepAction))
                        {
                            var parser = new Parser(reader);

                            while (parser.MoveNext() && !(parser.Current is MappingStart));

                            if (parser.Current is MappingStart)
                            {
                                int depth = 1;
                                while (depth > 0 && parser.MoveNext())
                                {
                                    if (parser.Current is MappingStart)
                                        depth++;
                                    else if (parser.Current is MappingEnd)
                                        depth--;

                                    if (depth > 0)
                                        emitter.Emit(parser.Current);
                                }
                            }
                            else
                            {
                                emitter.Emit(new Scalar(step.StepAction));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        emitter.Emit(new Scalar(step.StepAction));
                    }

                    emitter.Emit(new MappingEnd());
                    continue;
                }

                emitter.Emit(new Scalar("run"));
                emitter.Emit(new Scalar(step.StepAction));
                emitter.Emit(new MappingEnd());
            }
            emitter.Emit(new SequenceEnd());

            emitter.Emit(new MappingEnd());
        }

        emitter.Emit(new MappingEnd());
        emitter.Emit(new MappingEnd());
    }
}