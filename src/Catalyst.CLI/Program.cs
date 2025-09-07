using Catalyst.CLI;

public class Program
{
    public static void Main(string[] args)
    {

        if (args.Length == 0)
        {
            Console.WriteLine("Usage: catalyst <command> [options]");
            return;
        }

        var command = args[0];

        switch (command)
        {
            case "init":
                HandleInit(args.Skip(1).ToArray());
                break;
            case "build":
                HandleBuild(args.Skip(1).ToArray());
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
    }

    private static void HandleInit(string[] args)
    {
        // catalyst init [options]
        bool force = args.Contains("--force");
        string? path = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--path" && i + 1 < args.Length)
                path = args[i + 1];
        }

        Console.WriteLine($"Init command: force={force}, path={path ?? "default"}");
    }

    private static void HandleBuild(string[] args)
    {
        // catalyst build [OPTIONS]
        Console.WriteLine("Build completed");
    }
}