// ReSharper disable file MemberCanBePrivate.Global
//

namespace theredhead.common;

public static class StringExtensions
{
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

}
