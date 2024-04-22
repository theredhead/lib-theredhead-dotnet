using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace theredhead.common;

public class QueryString : IStringRepresentable
{
    private bool skipValuesEqualingTheirKey = true;
    public bool SkipValuesEqualingTheirKey { 
        get => skipValuesEqualingTheirKey; 
        set => skipValuesEqualingTheirKey = value;
    }
    public bool IncludeValuesEqualingTheirKey { 
        get => !skipValuesEqualingTheirKey; 
        set => skipValuesEqualingTheirKey = !value;
    }

    private Dictionary<string,string> keyValuePairs = new();

    public bool HasVariables => keyValuePairs.Any();

    public bool HaveVariable(string key) => keyValuePairs.ContainsKey(key);

    public string GetString(string key, string defaultValue = "") => HaveVariable(key) ? this[key] : defaultValue;
    public bool GetBoolean(string key, bool defaultValue = false) => HaveVariable(key) ? this[key] != "" : defaultValue;
    
    public int GetInteger(string key, int defaultValue = 0) => HaveVariable(key) ? this[key].AsInteger() : defaultValue;


    public string this[string key] {
        get => keyValuePairs[key];
        set => keyValuePairs[key] = value;
    }

    public bool TryGetValue(string key, out string? value) => keyValuePairs.TryGetValue(key, out value);
    public void Clear() {
        keyValuePairs.Clear();
    }
    public QueryString() {}
    public QueryString(string queryString) {
        if (!TryParse(queryString)) {
            throw new InvalidQueryStringException(queryString);
        }
    }
    public override string ToString() {
        var sb = new StringBuilder();
        foreach (var kvp in keyValuePairs) {
            sb.Append(kvp.Key);
            if (IncludeValuesEqualingTheirKey || kvp.Key != kvp.Value) {
                sb.Append("=");
                sb.Append(kvp.Value);
            }
            sb.Append("&");
        }
        return sb.ToString().TrimEnd('&');
    }
    public bool TryParse(string source) {
        foreach (var pair in source.Split('&')) {
            if (pair.TrySplitAtFirst("=", out var key, out var value)) {
                keyValuePairs[key] = value;
            } else if (pair.Length > 0) {
                keyValuePairs[pair] = pair;
            } else {
                return false;
            }
        }
        return true;
    }
    public static bool TryParse(string source, out QueryString? result) {
        result = new QueryString();
        return result.TryParse(source);
    }

    public string ToStringRepresentation() => ToString();

    public bool InitWithStringRepresentation(string representation) => TryParse(representation);
}

[Serializable]
internal class InvalidQueryStringException : Exception
{
    public string InvalidQueryString { get; private set; }
    private const string MESSAGE = "Invalid QueryString";

    public InvalidQueryStringException(string queryString) : base(MESSAGE)
    {
        InvalidQueryString = queryString;
    }

    public InvalidQueryStringException(string queryString, Exception? innerException) : base(MESSAGE, innerException)
    {
        InvalidQueryString = queryString;
    }
}