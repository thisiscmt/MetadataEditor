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
        Console.WriteLine("Metadata Editor supports the following opertions:");
        Console.WriteLine();
        Console.WriteLine(" -fc                List file names that violate casing rules for artist or song title.");

        // TODO

        break;
    default:
        Console.WriteLine("Unsupported parameter. Run 'medit -help' for more information.");

        break;
}
