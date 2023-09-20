
namespace DigitialLibrary;

internal static class App
{
    public static string Basepath { get; private set; }

    public static void Main(string[] args)
    {
        ParseArgs(args);

        Indexer indexer = new Indexer(Basepath);

        indexer.Start();
    }

    public static bool ParseArgs(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-p":
                    Basepath = args[i + 1];
                    i += 1;
                    break;
                default:
                    Console.WriteLine("No such option");
                    return false;
                    break;
            }
        }

        return true;
    }
}