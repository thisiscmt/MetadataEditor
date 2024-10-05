using ATL;
using MetadataEditor.Classes;
using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MetadataEditor
{
    internal class Operations
    {
        private const string DEFAULT_RESULTS_FILE = "Metadata Editor results.txt";

        #region Public static methods
        public static void RunCasingCheck(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                return;
            }

            try
            {
                string baseDirPath = args[1];
                string extension = args[2];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                string? result;
                var results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
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

                Utils.ProcessResults(files, results, outputFileArg);
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
                string baseDirPath = args[1];
                string extension = args[2];
                string searchText = args[3];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                var results = new List<string>();

                if (args.Length == 5)
                {
                    outputFileArg = args[4];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
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

                    if (track.TrackNumber.ToString()!.Contains(searchText))
                    {
                        results.Add($"{file.FullName} | {track.TrackNumber}");
                    }

                    if (track.DiscNumber.ToString()!.Contains(searchText))
                    {
                        results.Add($"{file.FullName} | {track.DiscNumber}");
                    }
                }

                Utils.ProcessResults(files, results, outputFileArg);
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
                string baseDirPath = args[1];
                string extension = args[2];
                string fieldName = args[3].ToLower();
                string checkToPerform = args[4];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                var results = new List<string>();

                if (args.Length == 6)
                {
                    outputFileArg = args[5];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
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

                Utils.ProcessResults(files, results, outputFileArg);
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
                string baseDirPath = args[1];
                string extension = args[2];
                string outputFileArg = DEFAULT_RESULTS_FILE;
                string? result;
                var results = new List<string>();

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
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

                Utils.ProcessResults(files, results, outputFileArg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunPlaylistGeneration(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                return;
            }

            try
            {
                string baseDirPath = args[1];
                string extension = args[2];
                string fullExtension = $"*.{extension}";
                int playlistCount = 0;

                DirectoryInfo baseDir = new DirectoryInfo(baseDirPath);
                BuildPlaylistFromDirectory(baseDir, fullExtension, ref playlistCount);

                Console.WriteLine($"\nPlaylists created: {playlistCount:N0}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunMasterPlaylistGeneration(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                return;
            }

            try
            {
                string baseDirPath = args[1];
                string extension = args[2];
                string fullExtension = $"*.{extension}";
                int playlistCount = 0;

                string[] exclusions = ["Archive", "Ben Folds", "Cocteau Twins", "David Bowie", "Howard Jones", "Joe Jackson", "John Stewart", 
                    "Kitchens of Distinction", "LTJ Bukem", "Lush", "Neil Finn", "Orbital", "Peter Gabriel", "Split Enz", "Squeeze", "Sting", "The Cure",
                    "They Might Be Giants", "XTC"];

                var baseDir = new DirectoryInfo(baseDirPath);
                var artistDirs = baseDir.EnumerateDirectories();

                foreach (DirectoryInfo artistDir in artistDirs)
                {
                    if (!exclusions.Contains(artistDir.Name))
                    {
                        var albumDirs = artistDir.EnumerateDirectories();

                        if (albumDirs.Any())
                        {
                            List<AlbumDirectory> albumsByYear = new List<AlbumDirectory>();

                            foreach (DirectoryInfo albumDir in albumDirs)
                            {
                                FileInfo file;
                                var files = albumDir.EnumerateFiles($"*.{extension}").ToList();

                                if (files.Any())
                                {
                                    file = files.First();
                                    Track track = new Track(file.FullName);

                                    if (track.Year != null)
                                    {
                                        albumsByYear.Add(new AlbumDirectory(albumDir, artistDir.Name, track.Year.Value.ToString()));
                                    }
                                }
                            }

                            if (albumsByYear.Any())
                            {
                                albumsByYear = albumsByYear.OrderBy(x => x.Year).ToList();
                                string playlistFilePath = Path.Combine($"{artistDir.FullName}", $"{artistDir.Name}.m3u");

                                foreach (AlbumDirectory album in albumsByYear)
                                {
                                    if (album == albumsByYear.First())
                                    {
                                        BuildPlaylist(album.Directory, fullExtension, playlistFilePath, false, false, ref playlistCount);
                                    }
                                    else
                                    {
                                        BuildPlaylist(album.Directory, fullExtension, playlistFilePath, true, false, ref playlistCount);
                                    }
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"\nPlaylists created: {playlistCount:N0}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunSingleQuoteCheck(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                return;
            }

            try
            {
                string baseDirPath = args[1];
                string extension = args[2];
                string fullExtension = $"*.{extension}";
                string outputFileArg = DEFAULT_RESULTS_FILE;
                bool fileRenamed = false;
                bool metadataChanged = false;
                var results = new List<string>();
                int correctedFileCount = 0;

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
                Track track;
                string fileName;
                char rightSmartQuote = '\u2019';
                char rightStraightQuote = '\'';
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    if (file.Name.IndexOf(rightSmartQuote) > -1)
                    {
                        fileName = file.Name.Replace(rightSmartQuote, rightStraightQuote);
                        File.Move(file.FullName, Path.Combine(file.DirectoryName!, fileName));
                        fileRenamed = true;
                    }
                    else
                    {
                        fileName = file.Name;
                    }

                    track = new Track(Path.Combine(file.DirectoryName!, fileName));

                    if (track.Title.IndexOf(rightSmartQuote) > -1)
                    {
                        track.Title = track.Title.Replace(rightSmartQuote, rightStraightQuote);
                        metadataChanged = true;
                    }

                    if (track.Artist.IndexOf(rightSmartQuote) > -1)
                    {
                        track.Artist = track.Artist.Replace(rightSmartQuote, rightStraightQuote);
                        metadataChanged = true;
                    }

                    if (track.Album.IndexOf(rightSmartQuote) > -1)
                    {
                        track.Album = track.Album.Replace(rightSmartQuote, rightStraightQuote);
                        metadataChanged = true;
                    }

                    if (track.AlbumArtist.IndexOf(rightSmartQuote) > -1)
                    {
                        track.AlbumArtist = track.AlbumArtist.Replace(rightSmartQuote, rightStraightQuote);
                        metadataChanged = true;
                    }

                    if (metadataChanged)
                    {
                        track.Save();
                    }

                    if (fileRenamed || metadataChanged)
                    {
                        results.Add(file.FullName);
                        correctedFileCount++;
                    }

                    fileRenamed = false;
                    metadataChanged = false;
                }

                Utils.ProcessResults(files, results, outputFileArg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunDateFieldCheck(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Missing required parameter. Run 'medit -help' for more information.");
                return;
            }

            try
            {
                string baseDirPath = args[1];
                string extension = args[2];
                string fullExtension = $"*.{extension}";
                string outputFileArg = DEFAULT_RESULTS_FILE;
                var results = new List<string>();
                int correctedFileCount = 0;

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
                Track track;
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    track = new Track(file.FullName);

                    if (track.Date != null)
                    {
                        track.Year = track.Date.Value.Year;
                        track.Save();

                        results.Add(file.FullName);
                        correctedFileCount++;
                    }
                }

                Utils.ProcessResults(files, results, outputFileArg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion

        #region Private static methods
        private static void BuildPlaylistFromDirectory(DirectoryInfo baseDir, string fullExtension, ref int playlistCount)
        {
            var dirs = baseDir.EnumerateDirectories();

            foreach (DirectoryInfo dir in dirs)
            {
                BuildPlaylistFromDirectory(dir, fullExtension, ref playlistCount);
            }

            string playlistFilePath = Path.Combine(baseDir.FullName, $"{baseDir.Name}.m3u");
            BuildPlaylist(baseDir, fullExtension, playlistFilePath, false, true, ref playlistCount);
        }

        private static void BuildPlaylist(DirectoryInfo dir, string fullExtension, string playlistFilePath, bool append, bool checkForExistingPlaylist, 
                                          ref int playlistCount)
        {
            if (checkForExistingPlaylist)
            {
                var playlistFiles = dir.EnumerateFiles("*.m3u");

                if (playlistFiles.Any())
                {
                    return;
                }
            }

            var files = dir.EnumerateFiles(fullExtension).OrderBy(x => new Track(x.FullName).TrackNumber);

            if (!files.Any())
            {
                return;
            }

            // Windows-1252 encoding is used here so the files work properly in Winamp.
            using StreamWriter playlistFile = new StreamWriter(playlistFilePath, append, Encoding.GetEncoding(1252));

            if (!append)
            {
                playlistFile.WriteLine("#EXTM3U");
            }

            foreach (FileInfo file in files)
            {
                Track track = new Track(file.FullName);
                string infEntry = $"#EXTINF:{track.Duration},{track.Artist} - {track.Title}";

                playlistFile.WriteLine(infEntry);

                // If we aren't doing a check for an existing playlist then we assume a master playlist is being created, and so the file location entry
                // needs to be prefixed with the name of the album directory.
                if (checkForExistingPlaylist)
                {
                    playlistFile.WriteLine(file.Name);
                }
                else
                {
                    playlistFile.WriteLine($"{dir.Name}\\{file.Name}");
                }
            }

            if (!append)
            {
                playlistCount++;
            }
        }
        #endregion
    }
}
