using System.Linq;

namespace  System
{

    public static class StringExtensions {
        public static string Summary(this String str, int numberOfwords)
        {
            var words = str.Split(' ');
            if(numberOfwords >= words.Length)
                return str;
            return string.Join(' ', words.ToList().Take(numberOfwords)) + "...";
        }
    }    
}