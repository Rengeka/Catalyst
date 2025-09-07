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
        Console.WriteLine("Initializing catalyst...");

        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Template");
        string targetPath = Path.Combine(Directory.GetCurrentDirectory(), "pipeline.csx");

        if (!File.Exists(templatePath))
        {
            Console.WriteLine("Template not found. Please check if Catalyst package is not corrupt");
            return;
        }

        if (File.Exists(targetPath))
        {
            Console.WriteLine("File pipeline.csx allready exists in target directory");
            return;
        }

        File.Copy(templatePath, targetPath);
        Console.WriteLine("pipeline.csx Was successfully created in the target directory");
    }

    private static void HandleBuild(string[] args)
    {
        // catalyst build [OPTIONS]
        Console.WriteLine("Build is cur3rentlty unsupported. Please check if the new version is available");
    }
}