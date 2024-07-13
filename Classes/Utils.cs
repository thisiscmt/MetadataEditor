namespace MetadataEditor
{
    internal class Utils
    {
        public static bool ContainsText(string source, string text)
        {
            bool artistPartMatch = false;
            bool titlePartMatch = false;
            bool match = false;

            if (source.Contains(" - "))
            {
                string[] fileNameParts = Path.GetFileNameWithoutExtension(source).Split(" - ");

                artistPartMatch = fileNameParts[0].Contains($" {text} ", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($" {text} (", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($") {text} ", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($": {text} ", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($", {text} ", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($". {text} ", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($"/ {text} ", StringComparison.CurrentCulture) &&
                                  !fileNameParts[0].Contains($"! {text} ", StringComparison.CurrentCulture);

                titlePartMatch = fileNameParts[1].Contains($" {text} ", StringComparison.CurrentCulture) && 
                                 !fileNameParts[1].Contains($" {text} (", StringComparison.CurrentCulture) && 
                                 !fileNameParts[1].Contains($") {text} ", StringComparison.CurrentCulture) &&
                                 !fileNameParts[1].Contains($": {text} ", StringComparison.CurrentCulture) &&
                                 !fileNameParts[1].Contains($", {text} ", StringComparison.CurrentCulture) &&
                                 !fileNameParts[1].Contains($". {text} ", StringComparison.CurrentCulture) &&
                                 !fileNameParts[1].Contains($"/ {text} ", StringComparison.CurrentCulture) &&
                                 !fileNameParts[1].Contains($"! {text} ", StringComparison.CurrentCulture);

                return artistPartMatch || titlePartMatch;
            }
            else
            {
                match = source.Contains($" {text} ", StringComparison.CurrentCulture) && 
                        !source.Contains($" {text} (", StringComparison.CurrentCulture) && 
                        !source.Contains($") {text} ", StringComparison.CurrentCulture) &&
                        !source.Contains($": {text} ", StringComparison.CurrentCulture) &&
                        !source.Contains($", {text} ", StringComparison.CurrentCulture) &&
                        !source.Contains($". {text} ", StringComparison.CurrentCulture) &&
                        !source.Contains($"/ {text} ", StringComparison.CurrentCulture) &&
                        !source.Contains($"! {text} ", StringComparison.CurrentCulture);

                return match;
            }
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

            if (ContainsText(valueToCheck, "A") || ContainsText(valueToCheck, "And") || ContainsText(valueToCheck, "As") ||
                ContainsText(valueToCheck, "At") || ContainsText(valueToCheck, "For") || ContainsText(valueToCheck, "In") || 
                ContainsText(valueToCheck, "Of") || ContainsText(valueToCheck, "On") || ContainsText(valueToCheck, "Or") || 
                ContainsText(valueToCheck, "The") || ContainsText(valueToCheck, "To"))
            {
                resultToAdd = Path.Combine(dirPath, fileName);
            }

            return resultToAdd;
        }
    }
}
