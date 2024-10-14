using ATL;
using System.Text;

namespace MetadataEditor
{
    internal class Utils
    {
        #region Private fields
        // These values are special cases that should never be flagged as being wrong. They are here to avoid adding a bunch of custom logic for one-offs
        // and to get an accurate result for each operation.
        private const string CUSTOM_CASE_1 = "OK Alright A Huh Oh Yeah";

        private static string[] m_lowerCaseWords = ["A", "And", "As", "At", "For", "In", "Of", "On", "Or", "The", "To"];

        private static Dictionary<char, string> m_specialChars = new Dictionary<char, string>
        {
            { '\u2018', "Left single quote" },
            { '\u2019', "Right single quote" },
            { '\u201C', "Left double quote" },
            { '\u201D', "Right double quote" },
            { '\u2033', "Double prime" },
            { '\u2010', "Hyphen" }
        };
        #endregion

        #region Public static methods

        public static void ProcessResults(List<FileInfo> files, List<string> results, string outputFileArg)
        {
            string formattedFilesChecked = files.Count.ToString("N0");

            if (results.Count == 0)
            {
                string filesChecked = files.Count.ToString("N0");
                Console.WriteLine($"\nNo errors were found. Files checked: {formattedFilesChecked}");

                return;
            }

            results.Sort();

            string outputFilePath = Utils.CreateOutputDirectory(outputFileArg);
            using TextWriter outputFile = File.CreateText(outputFilePath);

            foreach (string result in results)
            {
                outputFile.WriteLine(result);
            }

            Console.WriteLine($"\nFile \"{outputFilePath}\" created successfully. Files checked: {formattedFilesChecked}. Results found: {results.Count:N0}");
        }

        public static string? CheckCasing(string valueToCheck, string fileName, string dirPath)
        {
            string? resultToAdd = null;

            foreach (string word in Utils.m_lowerCaseWords)
            {
                if (ContainsWord(valueToCheck, word))
                {
                    resultToAdd = Path.Combine(dirPath, fileName);
                }
            }

            return resultToAdd;
        }

        public static string? CheckForLowerCaseAfterParen(string valueTocheck, string fileName, string dirPath)
        {
            string? resultToAdd = null;

            if (valueTocheck.Contains('('))
            {
                int index = valueTocheck.IndexOf('(');
                string wordAfterParensToCheck = valueTocheck.Substring(index + 1, 3).ToLower().Trim();

                if (!Utils.m_lowerCaseWords.Any(x => x.ToLower() == wordAfterParensToCheck))
                {
                    byte[] asciiValue = Encoding.ASCII.GetBytes([valueTocheck[index + 1]]);

                    if (asciiValue[0] >= 97 && asciiValue[0] <= 122)
                    {
                        resultToAdd = Path.Combine(dirPath, fileName);
                    }
                }
            }

            return resultToAdd;
        }

        public static string CheckForSpecialCharacter(string value)
        {
            foreach(char specialChar in m_specialChars.Keys)
            {
                if (value.Contains(specialChar))
                {
                    return m_specialChars[specialChar];
                }
            }

            return "";
        }
        public static void BuildPlaylistFromDirectory(DirectoryInfo baseDir, string fullExtension, ref int playlistCount)
        {
            var dirs = baseDir.EnumerateDirectories();

            foreach (DirectoryInfo dir in dirs)
            {
                BuildPlaylistFromDirectory(dir, fullExtension, ref playlistCount);
            }

            string playlistFilePath = Path.Combine(baseDir.FullName, $"{baseDir.Name}.m3u");
            BuildPlaylist(baseDir, fullExtension, playlistFilePath, false, true, ref playlistCount);
        }

        public static void BuildPlaylist(DirectoryInfo dir, string fullExtension, string playlistFilePath, bool append, bool checkForExistingPlaylist,
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

        #region Private static methods
        public static string CreateOutputDirectory(string outputFileArg)
        {
            string outputFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), outputFileArg);

            if (outputFileArg.Contains(Path.DirectorySeparatorChar))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFileArg)!);
                outputFilePath = outputFileArg;
            }

            return outputFilePath;
        }

        private static bool ContainsWord(string source, string word)
        {
            bool artistPartMatch;
            bool titlePartMatch;
            bool match;

            if (source.Contains(" - "))
            {
                string[] fileNameParts = Path.GetFileNameWithoutExtension(source).Split(" - ");

                artistPartMatch = ContainsText(fileNameParts[0], word);
                titlePartMatch = ContainsText(fileNameParts[1], word);

                return (artistPartMatch || titlePartMatch) && !IsFourPartInOutTitle(fileNameParts[1]) && fileNameParts[1] != CUSTOM_CASE_1;
            }
            else
            {
                match = ContainsText(source, word);

                return match && !IsFourPartInOutTitle(source) && source != CUSTOM_CASE_1;
            }
        }

        private static bool ContainsText(string source, string text)
        {
            return source.Contains($" {text} ", StringComparison.CurrentCulture) && !source.Contains($" {text} (", StringComparison.CurrentCulture) &&
                   !source.Contains($") {text} ", StringComparison.CurrentCulture) && !source.Contains($": {text} ", StringComparison.CurrentCulture) &&
                   !source.Contains($", {text} ", StringComparison.CurrentCulture) && !source.Contains($". {text} ", StringComparison.CurrentCulture) &&
                   !source.Contains($"/ {text} ", StringComparison.CurrentCulture) && !source.Contains($"! {text} ", StringComparison.CurrentCulture);
        }

        private static bool IsFourPartInOutTitle(string valueToCheck)
        {
            // We check for the case of a 4-part title of the form '<word> In <word> Out'. In this case we want to ignore the regular casing rules because
            // it would look and read better with the In as a primary word. An example is 'Drive In Drive Out' by The Dave Matthews Band.
            string[] titleParts = valueToCheck.Split(" ");

            return titleParts.Length == 4 && titleParts[1] == "In" && titleParts[3] == "Out";
        }
    }
    #endregion
}
