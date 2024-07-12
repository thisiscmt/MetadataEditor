using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataEditor
{
    internal class Utils
    {
        public static bool FileNameContainsText(string source, string text)
        {
            string[] fileNameParts = Path.GetFileNameWithoutExtension(source).Split(" - ");
            bool artistPartMatch = fileNameParts[0].Contains($" {text} ", StringComparison.CurrentCulture) && !fileNameParts[0].Contains($" {text} (", StringComparison.CurrentCulture);
            bool titlePartMatch = fileNameParts[1].Contains($" {text} ", StringComparison.CurrentCulture) && !fileNameParts[1].Contains($" {text} (", StringComparison.CurrentCulture);

            return artistPartMatch || titlePartMatch;
        }
    }
}
