﻿using MetadataEditor;

if (args.Length == 0)
{
    Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
    Environment.Exit(0);

    return;
}

switch(args[0])
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

        break;
    default:
        Console.WriteLine("Unsupported parameter. Run 'medit -help' for more information.");

        break;
}
