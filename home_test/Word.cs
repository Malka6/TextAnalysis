using System.Collections.Generic;

namespace TextAnalysis
{
    public static class Word
    {
        public static bool IsOnlyBrackets(string word, char s)
        {
            var ends = new List<string> { $"({s})", $"[{s}]", $"{{{s}}}", $"\"{s}\"", $"'{s}'", $"<{s}>" };
            return ends.Exists(str => word.EndsWith(str));
        }

        public static bool IsEndOfSentence(string word, int index)
        {
            List<char> charsForEnd = new List<char> { '.', '?', '!', ':', ';' };
            List<char> closingBracketList = new List<char> { ')', ']', '}', '"', '\'', '>' };

            if (word.Length > 2 || index != 0)//Ignore 1. or 1)
            {
                if (charsForEnd.Exists(str => IsOnlyBrackets(word, str)))
                    return false;

                for (int i = word.Length - 1; i >= 0; i--)
                {
                    if (charsForEnd.Contains(word[i]))
                        return true;
                    if (closingBracketList.Contains(word[i]))
                        continue;
                    return false;
                }
            }

            return false;
        }
    }
}
