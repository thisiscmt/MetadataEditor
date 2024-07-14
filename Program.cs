using MetadataEditor;

if (args.Length == 0)
{
    Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
    Environment.Exit(0);

    return;
}

switch(args[0])
{
    case "-filecasing":
    case "-fc":
        Operations.RunFileCasing(args);

        break;
    case "-metacasing":
    case "-mc":
        Operations.RunMetadataCasing(args);

        break;
    case "-help":
    case "-h":
        Console.WriteLine("Metadata Editor does various tasks involving music files and their metadata. Commands of are of the form:");
        Console.WriteLine();
        Console.WriteLine("medit <task> <path to music files> <file extension> <path to output file> (optional)");
        Console.WriteLine();
        Console.WriteLine("Paths with spaces must be enclosed in quotes. All subdirectories within the path to the music files will be checked for the given extension. If the output file parameter is omitted, a file will be created in the Documents directory. The following tasks are supported:");
        Console.WriteLine();
        Console.WriteLine(" -fc     Generate a list of files whose name violates casing rules. File names are assumed to follow this standard: '<artist> - <title>'.");
        Console.WriteLine(" -mc     Generate a list of files that have an artist, title, or album metadata value that violates casing rules.");

        break;
    default:
        Console.WriteLine("Unsupported parameter. Run 'medit -help' for more information.");

        break;
}
