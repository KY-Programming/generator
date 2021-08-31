using System.Text.RegularExpressions;

namespace KY.Generator.Models
{
    public class FileNameReplacer
    {
        /// <summary>
        /// Optional key for others to find a replacer and change its behaviour
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// A regex pattern like from <see cref="Regex.Replace(string,string)"/>
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// A regex replacement like from <see cref="Regex.Replace(string,string)"/> e.g. "$1..."
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        /// The required type of the file. Default is null. Null means all
        /// </summary>
        public string MatchingType { get; set; }

        public FileNameReplacer(string key, string pattern, string replacement, string matchingType = null)
        {
            this.Key = key;
            this.Pattern = pattern;
            this.Replacement = replacement;
            this.MatchingType = matchingType;
        }
    }
}
