using System;

namespace MetadataEditor
{
    internal class Utils
    {
        public static bool FileNameContainsText(string source, string text)
        {
            string[] fileNameParts = Path.GetFileNameWithoutExtension(source).Split(" - ");
            bool artistPartMatch = fileNameParts[0].Contains($" {text} ", StringComparison.CurrentCulture) && !fileNameParts[0].Contains($" {text} (", StringComparison.CurrentCulture) && !fileNameParts[0].Contains($") {text} ", StringComparison.CurrentCulture);
            bool titlePartMatch = fileNameParts[1].Contains($" {text} ", StringComparison.CurrentCulture) && !fileNameParts[1].Contains($" {text} (", StringComparison.CurrentCulture) && !fileNameParts[1].Contains($") {text} ", StringComparison.CurrentCulture);

            return artistPartMatch || titlePartMatch;
        }


        public static string CreateOutputDirectory(string[] args, string outputFileArg)
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
            using (TextWriter outputFile = File.CreateText(outputFilePath))
            {
                foreach (string result in results)
                {
                    outputFile.WriteLine(result);
                }
            }
        }

        public static string? CheckCasing(string valueToCheck, string fileName, string dirPath)
        {
            string? resultToAdd = null;

            if (Utils.FileNameContainsText(valueToCheck, "A") || Utils.FileNameContainsText(valueToCheck, "And") || Utils.FileNameContainsText(valueToCheck, "As") ||
                Utils.FileNameContainsText(valueToCheck, "At") || Utils.FileNameContainsText(valueToCheck, "For") || Utils.FileNameContainsText(valueToCheck, "In") || 
                Utils.FileNameContainsText(valueToCheck, "Of") || Utils.FileNameContainsText(valueToCheck, "On") || Utils.FileNameContainsText(valueToCheck, "Or") || 
                Utils.FileNameContainsText(valueToCheck, "The") || Utils.FileNameContainsText(valueToCheck, "To"))
            {
                resultToAdd = Path.Combine(dirPath, fileName);
            }

            return resultToAdd;
        }
    }
}
