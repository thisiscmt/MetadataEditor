﻿using System.Collections.Immutable;
using ATL;
using MetadataEditor.Classes;

namespace MetadataEditor
{
    internal class Operations
    {
        private const string DEFAULT_RESULTS_FILE = "Metadata Editor results.txt";
        private const string ARTIST_EXCLUSIONS_FILE = "Artist exclusions.txt";

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

                Utils.ProcessResults(files.Count, results, outputFileArg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunCasingParensCheck(string[] args)
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

                Utils.ProcessResults(files.Count, results, outputFileArg);
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

                Utils.ProcessResults(files.Count, results, outputFileArg);
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
                Track track;
                var results = new List<string>();

                if (args.Length == 6)
                {
                    outputFileArg = args[5];
                }

                DirectoryInfo dir = new DirectoryInfo(baseDirPath);
                var files = dir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in files)
                {
                    track = new Track(Path.Combine(file.DirectoryName!, file.Name));

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
                                results.Add(file.FullName);
                            }

                            break;
                        case "lyrics":
                            if ((checkToPerform == "empty" && track.Lyrics.SynchronizedLyrics.Count == 0 && track.Lyrics.UnsynchronizedLyrics.Trim() == "") || 
                                (checkToPerform == "nonempty" && (track.Lyrics.SynchronizedLyrics.Count > 0 || track.Lyrics.UnsynchronizedLyrics.Trim() != "")))
                            {
                                results.Add(file.FullName);
                            }

                            break;
                        case "artwork":
                            if ((checkToPerform == "empty" && track.EmbeddedPictures.Count == 0) || (checkToPerform == "nonempty" && track.EmbeddedPictures.Count > 0))
                            {
                                results.Add(file.FullName);
                            }

                            break;
                    }
                }

                Utils.ProcessResults(files.Count, results, outputFileArg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void RunSpecialCharacterCheck(string[] args)
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

                if (args.Length == 4)
                {
                    outputFileArg = args[3];
                }

                DirectoryInfo baseDir = new DirectoryInfo(baseDirPath);
                Track track;
                string specialCharFound;

                var musicFiles = baseDir.EnumerateFiles($"*.{extension}", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in musicFiles)
                {
                    specialCharFound = Utils.CheckForSpecialCharacter(file.Name);

                    if (specialCharFound != "")
                    {
                        results.Add($"{file.FullName} | {specialCharFound}");
                        continue;
                    }

                    track = new Track(Path.Combine(file.DirectoryName!, file.FullName));
                    specialCharFound = Utils.CheckForSpecialCharacter(track.Title);

                    if (specialCharFound != "")
                    {
                        results.Add($"{file.FullName} | Title | {specialCharFound}");
                        continue;
                    }

                    specialCharFound = Utils.CheckForSpecialCharacter(track.Album);

                    if (specialCharFound != "")
                    {
                        results.Add($"{file.FullName} | Album | {specialCharFound}");
                    }
                }

                var playlistFiles = baseDir.EnumerateFiles($"*.m3u", SearchOption.AllDirectories).ToList();

                foreach (FileInfo file in playlistFiles)
                {
                    specialCharFound = Utils.CheckForSpecialCharacter(file.Name);

                    if (specialCharFound != "")
                    {
                        results.Add($"{file.FullName} | {specialCharFound}");
                    }
                }

                var dirs = baseDir.EnumerateDirectories("*.*", SearchOption.AllDirectories).ToList();

                foreach (DirectoryInfo dir in dirs)
                {
                    specialCharFound = Utils.CheckForSpecialCharacter(dir.Name);

                    if (specialCharFound != "")
                    {
                        results.Add($"{dir.FullName} | {specialCharFound}");
                    }
                }

                Utils.ProcessResults(musicFiles.Count + playlistFiles.Count + dirs.Count, results, outputFileArg);
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
                Utils.BuildPlaylistFromDirectory(baseDir, fullExtension, ref playlistCount);

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
                string[] artistExclusions = [];
                string? currentAssemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var baseDir = new DirectoryInfo(baseDirPath);
                var artistDirs = baseDir.EnumerateDirectories();

                if (File.Exists(Path.Combine(currentAssemblyDir!, ARTIST_EXCLUSIONS_FILE)))
                {
                    string exclusionsFileContent = File.ReadAllText(ARTIST_EXCLUSIONS_FILE);
                    artistExclusions = exclusionsFileContent.Split(',', StringSplitOptions.TrimEntries);
                }

                foreach (DirectoryInfo artistDir in artistDirs)
                {
                    if (!artistExclusions.Contains(artistDir.Name))
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
                                        Utils.BuildPlaylist(album.Directory, fullExtension, playlistFilePath, false, false, ref playlistCount);
                                    }
                                    else
                                    {
                                        Utils.BuildPlaylist(album.Directory, fullExtension, playlistFilePath, true, false, ref playlistCount);
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

                Utils.ProcessResults(files.Count, results, outputFileArg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion
    }
}
