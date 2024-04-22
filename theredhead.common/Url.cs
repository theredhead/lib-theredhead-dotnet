namespace theredhead.core;

public class Url
{
    public class QueryString
    {
        private readonly Dictionary<string, string> variables = new ();

        public QueryString()
        {
        }
        public QueryString(string query)
        {
            Sanity.Enforce(TryParse(query), "Invalid query string");
        }

        public bool TryGetValue(string key, out string value)
        {
            return variables.TryGetValue(key, out value);
        }

        public bool TryParse(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return false;

            variables.Clear();

            var pairs = query.Split('&');
            foreach (var pair in pairs)
            {
                var parts = pair.Split('=');
                if (parts.Length != 2) return false;

                variables[parts[0]] = parts[1];
            }

            return true;
        }

        public static bool TryParse(string query, out QueryString queryString)
        {
            queryString = new QueryString();
            return  queryString.TryParse(query);
        }
    }

}
