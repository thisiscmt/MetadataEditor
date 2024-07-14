namespace MetadataEditor
{
    internal class Utils
    {
        public static bool ContainsText(string source, string text)
        {
            bool artistPartMatch;
            bool titlePartMatch;
            bool match;

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

                return (artistPartMatch || titlePartMatch) && !IsFourPartInOutTitle(fileNameParts[1]);
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

                return match && !IsFourPartInOutTitle(source);
            }
        }

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

            if (ContainsText(valueToCheck, "A") || ContainsText(valueToCheck, "And") || ContainsText(valueToCheck, "As") ||
                ContainsText(valueToCheck, "At") || ContainsText(valueToCheck, "For") || ContainsText(valueToCheck, "In") || 
                ContainsText(valueToCheck, "Of") || ContainsText(valueToCheck, "On") || ContainsText(valueToCheck, "Or") || 
                ContainsText(valueToCheck, "The") || ContainsText(valueToCheck, "To"))
            {
                resultToAdd = Path.Combine(dirPath, fileName);
            }

            return resultToAdd;
        }

        public static bool IsFourPartInOutTitle(string valueToCheck)
        {
            // We check for the case of a 4-part title of the form '<word> In <word> Out'. In this case we want to ignore the regular casing rules because
            // it would look and read better with the In as a primary word. An example is 'Drive In Drive Out' by The Dave Matthews Band.
            string[] titleParts = valueToCheck.Split(" ");

            return titleParts.Length == 4 && titleParts[1] == "In" && titleParts[3] == "Out";
        }
    }
}
