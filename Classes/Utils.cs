using System.Text;

namespace MetadataEditor
{
    internal class Utils
    {
        #region Private fields
        // These values are special cases that should never be flagged as being wrong. They are here to avoid adding a bunch of custom logic for one-offs
        // and to get an accurate result for each operation.
        private const string CUSTOM_CASE_1 = "OK Alright A Huh Oh Yeah";
        #endregion

        #region Public static methods
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

        public static void WriteOutput(string outputFilePath, List<string> results)
        {
            using TextWriter outputFile = File.CreateText(outputFilePath);

            foreach (string result in results)
            {
                outputFile.WriteLine(result);
            }
        }

        public static string? CheckCasing(string valueToCheck, string fileName, string dirPath)
        {
            string? resultToAdd = null;
            string[] wordsToCheck = ["A", "And", "As", "At", "For", "In", "Of", "On", "Or", "The", "To"];

            foreach (string word in wordsToCheck)
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

                if (valueTocheck.Substring(index + 1, 2) != "of")
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
        #endregion

        #region Private static methods
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
