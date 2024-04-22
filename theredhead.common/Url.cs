using System.Text;

namespace theredhead.common;

public class Url : IStringRepresentable {
    public string Scheme { get; private set; } = "";
    public string Host { get; private set; } = "";
    public uint? Port { get; set; } = null;
    public string Path { get; private set; } = "";
    public QueryString Query { get; private set; } = new QueryString();
    public string Fragment { get; private set; } = "";

    public Url() {}
    public Url(string url) { 
        if (!TryParse(url)) {
            throw new InvalidUrlException(url);
        }
    }

    public bool TryParse(string source) {
        Scheme = "";
        Host = "";
        Path = "";
        Query.Clear();
        Fragment = "";

        if (source.TrySplitAtFirst("://", out var scheme, out var rest)) {
            Scheme = scheme;
            if (rest.TrySplitAtFirst("/", out var hostAndPort, out var path)) {

                if (hostAndPort.TrySplitAtFirst(":", out var hostPart, out var portPart)) {
                    Host = hostPart;
                    if (uint.TryParse(portPart, out var port)) {
                        Port = port;
                    }
                } else {
                    Host = hostAndPort;
                }
                if (path.TrySplitAtFirst("?", out var pathPart, out var queryPart)) {
                    Path = pathPart;
                    if (queryPart.TrySplitAtFirst("#", out var query, out var fragment)) {
                        Query.TryParse(query).Or(Query.Clear);
                        Fragment = fragment;
                    } else {
                        Query.TryParse(queryPart).Or(Query.Clear);
                    }
                } else {
                    Path = path;
                }
            } else {
                Host = rest;
            }
            return true;
        }

        return false;
    }

    public static bool TryParse(string source, out Url? result) {
        result = new Url();
        return result.TryParse(source);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Scheme))
        {
            sb.Append(Scheme);
            sb.Append("://");
        }
        if (!string.IsNullOrWhiteSpace(Host))
        {
            sb.Append(Host);
            if (Port.HasValue)
            {
                sb.Append(":");
                sb.Append(Port);
            }
        }
        sb.Append("/");
        if (!string.IsNullOrWhiteSpace(Path))
        {
            sb.Append(Path);
        }
        if (Query.HasVariables)
        {
            sb.Append("?");
            sb.Append(Query);
        }
        if (!string.IsNullOrWhiteSpace(Fragment))
        {
            sb.Append("#");
            sb.Append(Fragment);
        }
        return sb.ToString();   
    }

    public string ToStringRepresentation() => ToString();

    public bool InitWithStringRepresentation(string representation) => TryParse(representation);
}

[Serializable]
public class InvalidUrlException : Exception
{
    public string InvalidUrl { get; private set; }
    private const string MESSAGE = "Invalid URL";
    public InvalidUrlException(string url) : base(MESSAGE)
    {
        InvalidUrl = url;
    }

    public InvalidUrlException(string url, Exception? innerException) : base(MESSAGE, innerException)
    {
        InvalidUrl = url;
    }
}
