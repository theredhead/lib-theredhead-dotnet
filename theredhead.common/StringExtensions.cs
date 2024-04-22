// ReSharper disable file MemberCanBePrivate.Global
//

using System.Globalization;
using theredhead.core;

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
}
