using ATL;

namespace MetadataEditor
{
    internal class Operations
    {
        private const string DEFAULT_FILE_CASING_FILE_NAME = "File casing errors.txt";
        private const string DEFAULT_METADATA_CASING_FILE_NAME = "Metadata casing errors.txt";

        public static void RunFileCasing(string[] args)
        {

            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                Environment.Exit(0);

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string outputFileArg = DEFAULT_FILE_CASING_FILE_NAME;
                List<string> results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    string? result = Utils.CheckCasing(file.Name, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add(result);
                    }
                }

                string formattedFilesChecked = files.Count.ToString("N0");

                if (results.Count == 0)
                {
                    Console.WriteLine($"No casing errors were found. Files checked: {formattedFilesChecked}");
                    Environment.Exit(0);
                }

                results.Sort();

                string outputFilePath = Utils.CreateOutputDirectory(args, outputFileArg);
                Utils.WriteOutput(outputFilePath, results);

                string formattedErrorsFound = results.Count.ToString("N0");
                Console.WriteLine($"File \"{outputFilePath}\" created successfully. Files checked: {formattedFilesChecked}. Casing errors found: {formattedErrorsFound}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunMetadataCasing(string[] args)
        {

            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                Environment.Exit(0);

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string outputFileArg = DEFAULT_METADATA_CASING_FILE_NAME;
                var results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
//                    Console.Write("\r{0}                                                                                           ", file.Name);

                    Track track = new Track(Path.Combine(file.DirectoryName!, file.Name));
                    string? result;
                        
                    result = Utils.CheckCasing(track.Artist, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.Artist}");
                        continue;
                    }

                    result = Utils.CheckCasing(track.Title, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.Title}");
                        continue;
                    }

                    result = Utils.CheckCasing(track.Album, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.Album}");
                        continue;
                    }
                }

                string formattedFilesChecked = files.Count.ToString("N0");

                if (results.Count == 0)
                {
                    string filesChecked = files.Count.ToString("N0");
                    Console.WriteLine($"\nNo casing errors were found. Files checked: {formattedFilesChecked}");

                    Environment.Exit(0);
                }

                results.Sort();

                string outputFilePath = Utils.CreateOutputDirectory(args, outputFileArg);
                Utils.WriteOutput(outputFilePath, results);

                string formattedErrorsFound = results.Count.ToString("N0");
                Console.WriteLine($"\nFile \"{outputFilePath}\" created successfully. Files checked: {formattedFilesChecked}. Casing errors found: {formattedErrorsFound}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
