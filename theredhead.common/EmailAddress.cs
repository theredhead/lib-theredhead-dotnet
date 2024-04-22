using System.Text;

namespace theredhead.common;


/// <summary>
/// Represents an email address with a display name, username, mailbox, and domain name.
/// like: "John Doe <john-doe+important@lost-and-found.com>"
/// </summary>
public class EmailAddress : IStringRepresentable
{
    public string DisplayName { get; set; } = "";
    public string Username { get; set; } = "";
    public string Mailbox { get; set; } = "";
    public string DomainName { get; set; } = "";

    public EmailAddress() { }
    public EmailAddress(string emailAddress)
    {
        if (!TryParse(emailAddress))
        {
            throw new InvalidEmailAddressException(emailAddress);
        }
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        var shouldUseAngleBrackets = false;
        if (!string.IsNullOrWhiteSpace(DisplayName))
        {
            shouldUseAngleBrackets = true;
            sb.Append(DisplayName);
            sb.Append(" ");
        }
        if (shouldUseAngleBrackets) sb.Append("<");
        sb.Append(Username);
        if (!string.IsNullOrWhiteSpace(Mailbox))
        {

            sb.Append("+");
            sb.Append(Mailbox);
        }
        sb.Append("@");
        sb.Append(DomainName);
        if (shouldUseAngleBrackets)sb.Append(">");
        return sb.ToString();
    }

    public bool TryParse(string source)
    {
        DisplayName = "";
        Username = "";
        Mailbox = "";
        DomainName = "";

        if (string.IsNullOrWhiteSpace(source))
        {
            return false;
        }

        // "John Doe <john-doe+important@lost-and-found.com>" => "John Doe", "<john-doe+important@lost-and-found.com>"
        if (source.TrySplitAtLast(" ", out var displayValue, out var addressValue))
        {
            DisplayName = displayValue.Trim();
        } else {
            addressValue = source;
        }
        var unrappedCompleteAddress = addressValue.TrimStart('<').TrimEnd('>');

        // "john-doe+important@lost-and-found.com" => "john-doe+important", "lost-and-found.com"
        if (unrappedCompleteAddress.TrySplitAtFirst("@", out var usernameAndMailbox, out var domainValue))
        {
            Username = usernameAndMailbox;
            DomainName = domainValue;

            // "john-doe+important" => "john-doe", "important"
            if (usernameAndMailbox.TrySplitAtFirst("+", out var usernameValue, out var mailboxName))
            {
                Username = usernameValue;
                Mailbox = mailboxName;
            }
            return true;
        }
        return false;
    }

    public static bool TryParse(string source, out EmailAddress result)
    {
        result = new EmailAddress();
        return result.TryParse(source);
    }

    public string ToStringRepresentation() => ToString();

    public bool InitWithStringRepresentation(string representation) => TryParse(representation);
}

[Serializable]
public class InvalidEmailAddressException : Exception
{
    public string InvalidEmailAddress { get; private set; }
    private const string MESSAGE = "Invalid email address";
    public InvalidEmailAddressException(string emailAddress) : base(MESSAGE)
    {
        InvalidEmailAddress = emailAddress;
    }

    public InvalidEmailAddressException(string emailAddress, Exception? innerException) : base(MESSAGE, innerException)
    {
        InvalidEmailAddress = emailAddress;
    }
}