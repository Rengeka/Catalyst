using YamlDotNet.Serialization;

namespace Catalyst.Github.Serializers;

public class KebabCaseNamingConvention : INamingConvention
{
    public string Apply(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        var builder = new System.Text.StringBuilder();
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                    builder.Append('-');
                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                builder.Append(c);
            }
        }
        return builder.ToString();
    }

    public string Reverse(string value)
    {
        throw new NotImplementedException();
    }
}