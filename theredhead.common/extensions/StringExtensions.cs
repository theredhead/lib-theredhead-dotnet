// ReSharper disable file MemberCanBePrivate.Global
//

using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using theredhead.common;

namespace theredhead.common;

public static class StringExtensions
{
    #region TrySplitAt...
    /// <summary>
    /// Attempt to split a string into the parts to the left and to the right of a given splitter
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="splitter"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool TrySplitAtFirst(this string subject, string splitter,
        out string left, out string right)
    {
        return subject.TrySplitAtFirst(splitter, StringComparison.Ordinal, out left, out right);
    }
    /// <summary>
    /// Attempt to split a string into the parts to the left and to the right of a given splitter
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="splitter"></param>
    /// <param name="comparison"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool TrySplitAtFirst(this string subject, string splitter, StringComparison comparison, out string left, out string right)
    {
        var index = subject.IndexOf(splitter, comparison);
        if (index > -1)
        {
            left = subject.Substring(0, index);
            right = subject.Substring(index + splitter.Length);
            return true;
        }
        left = "";
        right = "";
        return false;
    }
    
    /// <summary>
    /// Attempt to split a string into the parts to the left and to the right of a given splitter
    /// </summary>
    /// <param name="self"></param>
    /// <param name="splitter"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool TrySplitAtFirst(this string self, char splitter, out string left, out string right) {
        return TrySplitAtFirst(self, splitter.ToString(), out left, out right);
    }

    /// <summary>
    /// Attempt to split a string into the parts to the left and to the right of a given splitter
    /// </summary>
    /// <param name="self"></param>
    /// <param name="splitter"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool TrySplitAtLast(this string self, char splitter, out string left, out string right) {
        return TrySplitAtLast(self, splitter.ToString(), out left, out right);
    }

    /// <summary>
    /// Attempt to split a string into the parts to the left and to the right of a given splitter
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="splitter"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool TrySplitAtLast(this string subject, string splitter,
        out string left, out string right)
    {
        return subject.TrySplitAtLast(splitter, StringComparison.Ordinal, out left, out right);
    }
    /// <summary>
    /// Attempt to split a string into the parts to the left and to the right of a given splitter
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="splitter"></param>
    /// <param name="comparison"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool TrySplitAtLast(this string subject, string splitter, StringComparison comparison, out string left, out string right)
    {
        var index = subject.LastIndexOf(splitter, comparison);
        if (index > -1)
        {
            left = subject.Substring(0, index);
            right = subject.Substring(index + splitter.Length);
            return true;
        }
        left = "";
        right = "";
        return false;
    }
    #endregion TrySplitAt...

    /// <summary>
    /// Determines if a string contains only characters in a given set
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="validCharacters"></param>
    /// <param name="caseSensitive"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static bool ContainsOnlyCharactersIn(this string subject, string validCharacters, bool caseSensitive = true, CultureInfo? culture = null)
    {
        Sanity.EnforceNotNull(validCharacters, "validCharacters is null");

        culture ??= CultureInfo.InvariantCulture;
        return caseSensitive
            ? subject.ToCharArray().All(validCharacters.Contains)
            : subject.ToLower(culture).ToCharArray().All(validCharacters.ToLower(culture).Contains);
    }

    /// <summary>
    /// Determines if a string starts with any of the characters in a given set
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="validCharacters"></param>
    /// <param name="caseSensitive"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static bool StartsWithOneOf(this string subject, string validCharacters, bool caseSensitive = true,
        CultureInfo? culture = null)
    {
        Sanity.EnforceNotNull(validCharacters, "validCharacters is null");

        return (!string.IsNullOrWhiteSpace(subject)) 
            && subject.Substring(0, 1).ContainsOnlyCharactersIn(validCharacters, caseSensitive, culture);
    }

    /// <summary>
/// Determines if a string ends with any of the characters in a given set
/// </summary>
/// <param name="subject"></param>
/// <param name="validCharacters"></param>
/// <param name="caseSensitive"></param>
/// <param name="culture"></param>
/// <returns></returns>
    public static bool EndsWithOneOf(this string subject, string validCharacters, bool caseSensitive = true,
        CultureInfo? culture = null)
    {
        Sanity.EnforceNotNull(validCharacters, "validCharacters is null");

        return (!string.IsNullOrWhiteSpace(subject)) 
            && subject.Substring(subject.Length -1).ContainsOnlyCharactersIn(validCharacters, caseSensitive, culture);
    }

    /// <summary>
    /// Format a string with the properties of an object
    /// </summary>
    /// <param name="format"></param>
    /// <param name="context"></param>
    /// <param name="nullReplacement"></param>
    /// <returns></returns>
    static public string Fmt(this string format, object context, string nullReplacement = "NULL")
    {
        Sanity.EnforceNotNull(context, "context is null");
        Sanity.EnforceNotNull(nullReplacement, "nullReplacement is null");

        var type = context.GetType();
        var properties = type.GetProperties();

        var result = format;
        foreach (var property in properties)
        {
            var placeholder = "{" + property.Name + "}";
            if (!result.Contains(placeholder)) continue;

            var value = property.GetValue(context);
            result = result.Replace(placeholder, value?.ToString() ?? nullReplacement);
        }

        return result;
    }

    #region Inflection
    public static string ToCamelCase(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }
        else if (self.Length == 1)
        {
            return self.ToLower();
        }
        else
        {
            return char.ToLower(self[0]) + self.Substring(1);
        }
    }

    public static string ToPascalCase(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }
        else if (self.Length == 1)
        {
            return self.ToUpper();
        }
        else
        {
            return char.ToUpper(self[0]) + self.Substring(1);
        }
    }

    public static string ToSnakeCase(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }
        else
        {
            return string.Concat(self.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }

    public static string ToKebabCase(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }
        else
        {
            return string.Concat(self.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
        }
    }

    public static string ToTitleCase(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }
        else
        {
            return string.Concat(self.Select((x, i) => i == 0 || i > 0 && char.IsUpper(x) ? x.ToString() : x.ToString().ToLower()));
        }
    }

    public static string ToSentenceCase(this string self)
    {
        if (string.IsNullOrEmpty(self))
        {
            return self;
        }
        else
        {
            return string.Concat(self.Select((x, i) => i == 0 || i > 0 && char.IsUpper(x) ? x.ToString() : x.ToString().ToLower()));
        }
    }
   
    #endregion Inflection

    public static bool TrySplitAtFirstUnescaped(this string self, char splitter, out string left, out string right) {
        var pos = 0;
        while (pos < self.Length) {
            var ix = self.IndexOf(splitter, pos);
            if (self.CharAtIndexIsEscaped(ix, new char[] { splitter })) {
                pos += 2;
            } else if (self[pos] == splitter) {
                left = self.Substring(0, pos);
                right = self.Substring(pos + 1);
                return true;
            } else {
                pos++;
            }
        }
        left = self;
        right = "";
        return false;
    }
    public static bool CharAtIndexIsEscaped(this string self, int index, char[] escapableChars, char escapeChar = '\\') {
        if (index > 0 && index < self.Length) {
            var ch = self[index];
            if (escapableChars.Contains(ch)) {
                return index == 1 
                    ? self[index - 1] == escapeChar
                    : self[index - 1] == escapeChar && self[index - 2] != escapeChar;
            }
        }
        return false;
    }
    public static bool CharAtIndexIsEscaped(this string self, int index, string escapableChars, char escapeChar = '\\') {
        return self.CharAtIndexIsEscaped(index, escapableChars.ToCharArray(), escapeChar);
    }

    public static bool TrySplitAtIndex(this string self, int index, out string left, out string right) {
        if (index < 0 || index >= self.Length) {
            left = self;
            right = "";
            return false;
        }
        left = self.Substring(0, index);
        right = self.Substring(index);
        return true;
    }

    public static string ReadCharacters(this string self, char[] characters) {
        var sb = new StringBuilder();
        foreach (var ch in self) {
            if (characters.Contains(ch)) {
                sb.Append(ch);
            } else {
                break;
            }
        }
        return sb.ToString();
    }
    public static string ReadCharacters(this string self, string characters) {
        return self.ReadCharacters(characters.ToCharArray());
    }


    public static bool AsBoolean(this string self, string[]? falseValues = null, CultureInfo? culture = null) {
        culture ??= CultureInfo.InvariantCulture;
        falseValues ??= ["0", "false", "no"];

        var isFalse = falseValues.Contains(self.ToLower(culture ?? CultureInfo.InvariantCulture));
        return ! isFalse;
    }

    public static int AsInteger(this string self, int orDefault = 0) {

        if (int.TryParse(self, out var result)) {
            return result;
        }
        return orDefault;
    }

    public static long AsLong(this string self, long orDefault = 0) {

        if (long.TryParse(self, out var result)) {
            return result;
        }
        return orDefault;
    }

    public static float AsFloat(this string self, float orDefault = 0) {

        if (float.TryParse(self, out var result)) {
            return result;
        }
        return orDefault;
    }

    public static decimal AsDecimal(this string self, decimal orDefault = 0) {

        if (decimal.TryParse(self, out var result)) {
            return result;
        }
        return orDefault;
    }

    public static T As<T>(this string self) where T : IStringRepresentable, new() {
        return new T().Also(it => it.InitWithStringRepresentation(self));
    }
}
