﻿using ATL;
using System.Text;

namespace MetadataEditor
{
    internal class Operations
    {
        private const string DEFAULT_RESULTS_FILE = "Metadata Editor results.txt";

        public static void RunCasingCheck(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                string? result;
                var results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
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

                    return;
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

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string searchText = args[3];
                string outputFileArg = DEFAULT_RESULTS_FILE;
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

                    if (track.TrackNumber.ToString() == searchText)
                    {
                        results.Add($"{file.FullName} | {track.TrackNumber}");
                    }

                    if (track.DiscNumber.ToString() == searchText)
                    {
                        results.Add($"{file.FullName} | {track.DiscNumber}");
                    }
                }

                string formattedFilesChecked = files.Count.ToString("N0");

                if (results.Count == 0)
                {
                    string filesChecked = files.Count.ToString("N0");
                    Console.WriteLine($"\nNo results were found. Files checked: {formattedFilesChecked}");

                    return;
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

        public static void RunFieldCheck(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string fieldName = args[3].ToLower();
                string checkToPerform = args[4];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                var results = new List<string>();

                if (args.Length == 6)
                {
                    outputFileArg = args[5];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    Track track = new Track(Path.Combine(file.DirectoryName!, file.Name));

                    switch (fieldName)
                    {
                        case "artist":
                            if ((checkToPerform == "empty" && track.Artist == "") || (checkToPerform == "nonempty" && track.Artist != ""))
                            {
                                results.Add(file.FullName);
                            }

                            break;
                        case "title":
                            if ((checkToPerform == "empty" && track.Title == "") || (checkToPerform == "nonempty" && track.Title != ""))
                            {
                                results.Add(file.FullName);
                            }

                            break;
                        case "album":
                            if ((checkToPerform == "empty" && track.Album == "") || (checkToPerform == "nonempty" && track.Album != ""))
                            {
                                results.Add(file.FullName);
                            }

                            break;
                        case "albumartist":
                            if ((checkToPerform == "empty" && track.AlbumArtist == "") || (checkToPerform == "nonempty" && track.AlbumArtist != ""))
                            {
                                results.Add(file.FullName);
                            }

                            break;
                        case "date":
                            if ((checkToPerform == "empty" && (!track.Date.HasValue || (track.Date.HasValue && track.Date.Value.ToShortDateString() == "1/1/0001")) || 
                                (checkToPerform == "nonempty" && track.Date.HasValue && track.Date.Value.ToShortDateString() != "1/1/0001")))
                            {
                                if (track.Date.HasValue)
                                {
                                    results.Add($"{file.FullName} | {track.Date.Value.ToLongDateString()}");
                                }
                                else
                                {
                                    results.Add(file.FullName);
                                }
                            }

                            break;
                        case "genre":
                            if ((checkToPerform == "empty" && track.Genre == "") || (checkToPerform == "nonempty" && track.Genre != ""))
                            {
                                results.Add($"{file.FullName} | {track.Genre}");
                            }

                            break;
                    }
                }

                string formattedFilesChecked = files.Count.ToString("N0");

                if (results.Count == 0)
                {
                    string filesChecked = files.Count.ToString("N0");
                    Console.WriteLine($"\nNo results were found. Files checked: {formattedFilesChecked}");

                    return;
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

        public static void RunParensCheck(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");

                return;
            }

            try
            {
                string baseDir = args[1];
                string extension = args[2];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                string? result;
                var results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDir);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    result = Utils.CheckForLowerCaseAfterParen(file.Name, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add(result);
                        continue;
                    }

                    Track track = new Track(Path.Combine(file.DirectoryName!, file.Name));

                    result = Utils.CheckForLowerCaseAfterParen(track.Artist, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.Artist}");
                        continue;
                    }

                    result = Utils.CheckForLowerCaseAfterParen(track.Title, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.Title}");
                        continue;
                    }

                    result = Utils.CheckForLowerCaseAfterParen(track.Album, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.Album}");
                        continue;
                    }

                    result = Utils.CheckForLowerCaseAfterParen(track.AlbumArtist, file.Name, file.DirectoryName!);

                    if (result != null)
                    {
                        results.Add($"{result} | {track.AlbumArtist}");
                        continue;
                    }
                }

                string formattedFilesChecked = files.Count.ToString("N0");

                if (results.Count == 0)
                {
                    string filesChecked = files.Count.ToString("N0");
                    Console.WriteLine($"\nNo results were found. Files checked: {formattedFilesChecked}");

                    return;
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
