using CommandLine;

namespace ical;

public static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CreateEvent>(args).WithParsed(e => e.Save());
    }
}