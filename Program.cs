using System.Text;

using MetadataEditor;

if (args.Length == 0)
{
    Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
    Environment.Exit(0);

    return;
}

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

switch (args[0])
{
    case "-casing":
    case "-c":
        Operations.RunCasingCheck(args);

        break;
    case "-search":
    case "-s":
        Operations.RunSearch(args);

        break;
    case "-field":
    case "-f":
        Operations.RunFieldCheck(args);

        break;
    case "-casingparens":
    case "-cp":
        Operations.RunParensCheck(args);

        break;
    case "-playlist":
    case "-p":
        Operations.RunPlaylistGeneration(args);

        break;
    case "-masterplaylist":
    case "-mp":
        Operations.RunMasterPlaylistGeneration(args);

        break;
    case "-specialcharacter":
    case "-sc":
        Operations.RunSpecialCharacterCheck(args);

        break;
    case "-date":
    case "-d":
        Operations.RunDateFieldCheck(args);

        break;
    case "-help":
    case "-h":
        Console.WriteLine("Metadata Editor does various tasks involving music files and their metadata.");
        Console.WriteLine();
        Console.WriteLine("Paths with spaces must be enclosed in quotes. All subdirectories within the path to the music files will be checked for the given extension. The extension parameter should not include a dot. File names are assumed to follow this standard: '<artist> - <title>'. If the output file parameter is omitted a file will be created in the Documents directory. The following tasks are supported:");
        Console.WriteLine();
        Console.WriteLine("Generate a list of files whose name or artist/title/album/album artist metadata value violates casing rules.");
        Console.WriteLine("  -c [path to files] [extension] [path to output file (optional)]");
        Console.WriteLine();
        Console.WriteLine("Generate a list of files whose name or artist/title/album/album artist/track #/disc # metadata value contains the given text (case-sensitive).");
        Console.WriteLine("  -s [path to files] [extension] [search text] [path to output file (optional)]");
        Console.WriteLine();
        Console.WriteLine("Generate a list of files with a metadata value for artist/title/album/album artist/date/genre that is either empty or non-empty.");
        Console.WriteLine("  -f [path to files] [extension] [field name] ['empty' | 'nonempty'] [path to output file (optional)]");
        Console.WriteLine();
        Console.WriteLine("Generate a list of files whose name or artist/title/album/album artist metadata value has a '(' followed by a lower-case word that is not allowed.");
        Console.WriteLine("  -cp [path to files] [extension] [path to output file (optional)]");
        Console.WriteLine();
        Console.WriteLine("Generate playlists in all sub-directories in a parent directory based on track number.");
        Console.WriteLine("  -p [path to directory] [extension]");
        Console.WriteLine();
        Console.WriteLine("Generate a master playlist containing tracks from all albums from each artist sub-directory in a parent directory.");
        Console.WriteLine("  -mp [path to directory] [extension]");
        Console.WriteLine();
        Console.WriteLine("Check for special characters in file names and metadata, such as smart single and double quotes and a hyphen.");
        Console.WriteLine("  -sc [path to directory] [extension] [path to output file (optional)]");
        Console.WriteLine();
        Console.WriteLine("Convert the Date metadata field from a 3-part date value to a 4-digit year value, if present.");
        Console.WriteLine("  -d [path to directory] [extension] [path to output file (optional)]");

        break;
    default:
        Console.WriteLine("Unsupported parameter. Run 'medit -help' for more information.");

        break;
}
