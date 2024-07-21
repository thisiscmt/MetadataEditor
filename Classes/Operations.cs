using ATL;
using System.Text;

namespace MetadataEditor
{
    internal class Operations
    {
        private const string DEFAULT_CASING_RESULTS_FILE = "Casing errors.txt";

        public static void RunCasing(string[] args)
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
                string outputFileArg = DEFAULT_CASING_RESULTS_FILE;
                var results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    string? result;
                    result = Utils.CheckCasing(file.Name, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add(result);
                        continue;
                    }

                    Track track = new Track(Path.Combine(file.DirectoryName!, file.Name));

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

                    result = Utils.CheckCasing(track.AlbumArtist, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.AlbumArtist}");
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

                string outputFilePath = Utils.CreateOutputDirectory(outputFileArg);
                Utils.WriteOutput(outputFilePath, results);

                string formattedErrorsFound = results.Count.ToString("N0");
                Console.WriteLine($"\nFile \"{outputFilePath}\" created successfully. Files checked: {formattedFilesChecked}. Casing errors found: {formattedErrorsFound}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunSearch(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                Environment.Exit(0);

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string searchText = args[3];
                string outputFileArg = DEFAULT_CASING_RESULTS_FILE;
                var results = new List<string>();

                if (args.Length == 5)
                {
                    outputFileArg = args[4];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    if (file.Name.Contains(searchText))
                    {
                        results.Add(file.FullName);
                        continue;
                    }

                    Track track = new Track(Path.Combine(file.DirectoryName!, file.Name));

                    if (track.Artist.Contains(searchText))
                    {
                        results.Add($"{file.FullName} | {track.Artist}");
                        continue;
                    }

                    if (track.Title.Contains(searchText))
                    {
                        results.Add($"{file.FullName} | {track.Title}");
                        continue;
                    }

                    if (track.Album.Contains(searchText))
                    {
                        results.Add($"{file.FullName} | {track.Album}");
                        continue;
                    }

                    if (track.AlbumArtist.Contains(searchText))
                    {
                        results.Add($"{file.FullName} | {track.Album}");
                    }
                }

                string formattedFilesChecked = files.Count.ToString("N0");

                if (results.Count == 0)
                {
                    string filesChecked = files.Count.ToString("N0");
                    Console.WriteLine($"\nNo results were found. Files checked: {formattedFilesChecked}");

                    Environment.Exit(0);
                }

                results.Sort();

                string outputFilePath = Utils.CreateOutputDirectory(outputFileArg);
                Utils.WriteOutput(outputFilePath, results);

                string formattedErrorsFound = results.Count.ToString("N0");
                Console.WriteLine($"\nFile \"{outputFilePath}\" created successfully. Files checked: {formattedFilesChecked}. Results found: {formattedErrorsFound}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
