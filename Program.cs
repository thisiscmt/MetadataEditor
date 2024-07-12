using System.IO;
using ATL;
using ATL.AudioData;

using MetadataEditor;

if (args.Length == 0)
{
    Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.") ;
    Environment.Exit(0);

    return;
}

string operation = args[0];

switch(operation)
{
    // This operation generates a list of all files that have non-standard word casing. The list takes the form of full paths to the given files.
    case "-filecasing":
    case "-fc":
        if (args.Length < 3)
        {
            Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
            Environment.Exit(0);

            return;
        }

        string baseDir = args[1];
        string extension = args[2];
        string outputFileArg = "File casing errors.txt";
        var results = new List<string>();

        if (args.Length == 4)
        {
            outputFileArg = args[3];
        }

        DirectoryInfo dir = new DirectoryInfo(baseDir);
        var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

        foreach (FileInfo file in files)
        {
            if (Utils.FileNameContainsText(file.Name, "A") || Utils.FileNameContainsText(file.Name, "And") || Utils.FileNameContainsText(file.Name, "As") ||
                Utils.FileNameContainsText(file.Name, "At") || Utils.FileNameContainsText(file.Name, "For") ||
                Utils.FileNameContainsText(file.Name, "In") || Utils.FileNameContainsText(file.Name, "Of") || Utils.FileNameContainsText(file.Name, "On") ||
                Utils.FileNameContainsText(file.Name, "Or") || Utils.FileNameContainsText(file.Name, "The") || Utils.FileNameContainsText(file.Name, "To"))
            {
                if (file.DirectoryName == null)
                {
                    results.Add(file.Name);
                }
                else
                {
                    results.Add(Path.Combine(file.DirectoryName, file.Name));
                }
            }
        }

        if (results.Count == 0)
        {
            Console.WriteLine("No casing errors were found. Files checked: {0}", files.Count.ToString("G"));
            return;
        }

        string outputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), outputFileArg);

        if (outputFileArg.Contains(Path.DirectorySeparatorChar))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputFileArg)!);
            outputFilePath = outputFileArg;
        }

        TextWriter outputFile = File.CreateText(outputFilePath);
        results.Sort();

        foreach(string result in results)
        {
            outputFile.WriteLine(result);
        }

        outputFile.Close();
        Console.WriteLine($"File \"{outputFilePath}\" created successfully. Files checked: {files.Count.ToString("G")}. Casing errors found: {results.Count.ToString("G")}");

        break;
    case "-help":
    case "-h":
        Console.WriteLine("Metadata Editor supports the following opertions:");
        Console.WriteLine();
        Console.WriteLine(" -fc                List file names that violate casing rules.");

        break;
    default:
        Console.WriteLine("Unsupported parameter. Run 'medit -help' for more information.");

        break;
}



