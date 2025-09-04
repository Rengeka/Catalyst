namespace Catalyst.Core;

public interface IStep
{
    public IStep SetAction(string stepAction);
    public IStep SetRawAction(string rawKey, string rawStepAction);
}