using Catalyst.Core.Enums;

namespace Catalyst.Core.Structs;

public class Trigger
{
    public string[] Branches {  get; set; }
    public TriggerType TriggerType { get; set; }    
}