using System.Text;

namespace theredhead.text;

public static class StringComparisonAdditions
{

    static public string StringByRemovingAllOccurrencesOf(this string s, char[] charactersToRemove)
    {
        var result = new StringBuilder();
        foreach (char character in s.ToCharArray())
        {
            if (charactersToRemove.Contains(character)) continue;
            result.Append(character);
        }
        return result.ToString();
    }

    static public bool IsEquivalentTo(this string s, string other, char[] ignoringCharacters)
    {
        if (other == null) return false;

        var a = StringByRemovingAllOccurrencesOf(s, ignoringCharacters);
        var b = StringByRemovingAllOccurrencesOf(other, ignoringCharacters);

        return a.Equals(b);
    }

    static public bool IsEquivalentToIgnoringWhitespace(this string s, string other)
    {
        return IsEquivalentTo(s, other, new char[] { ' ', '\t', '\n' });
    }
}

