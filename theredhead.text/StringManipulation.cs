using System;
namespace theredhead.text
{
    public class QuoteKind(string prefix, string suffix)
	{
        private static readonly QuoteKind _double = new("\"", "\"");
        private static readonly QuoteKind _single = new("'", "'");
        private static readonly QuoteKind _block = new("[", "]");
        private static readonly QuoteKind _backtick = new("`", "`");

        public static QuoteKind Double => _double;
        public static QuoteKind Single => _single;
        public static QuoteKind Block => _block;
        public static QuoteKind Backtick => _backtick;

        public string Quote(string subject)
		{
            return prefix + subject + suffix;
        }
        public string Unquote(string subject)
		{
            var startOffset = subject.StartsWith(prefix) ? prefix.Length : 0;
            var endOffset = subject.EndsWith(suffix) ? suffix.Length : 0;

            return subject.Substring(startOffset, subject.Length - startOffset - endOffset);
        }
    }


	static public class StringManipulation
	{
        public static string Quoted(this string subject, QuoteKind quotekind)
        {
            return quotekind.Quote(subject);
        }

        public static string Quoted(this string subject, string prefix, string suffix)
		{
			return prefix + subject + suffix;
		}

		public static string JoinedBy(this IEnumerable<string> strings, string glue)
		{
			return string.Join(glue, strings);
		}
	}
}

