using System.Data;
using System.Text;
using theredhead.common;

namespace theredhead.core;

/// <summary>
/// Represent an email address
/// </summary>
public class EmailAddress
{
    private const string DomainAllowableCharacters = "abcdefghijklmnopqrstuvwxyz1234567890_-.";
    public string DisplayName { get; set; } = "";
    public string Username { get; set; } = "";
    public string Mailbox { get; set; } = "";
    public string Domain { get; set; } = "";

    public EmailAddress()
    {
    }

    public EmailAddress(string emailAddress)
    {
        if (!TryParse(emailAddress))
        {
            throw new InvalidEmailAddressException(emailAddress);
        }
    }

    public void CopyFrom(EmailAddress other)
    {
        DisplayName = other.DisplayName;
        Username = other.Username;
        Mailbox = other.Mailbox;
        Domain = other.Domain;
    }

    public EmailAddress Copy(
        string? displayName = null,
        string? username = null,
        string? mailbox = null,
        string? domain = null)
    {
        var copy = new EmailAddress
        {
            DisplayName = displayName ?? DisplayName,
            Username = username ?? Username,
            Mailbox = mailbox ?? Mailbox,
            Domain = domain ?? Domain
        };
        return copy;
    }

    public bool TryParse(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        var state = new EmailAddress();
        state.CopyFrom(this);

        var subject = email;
        // "John Doe <jd+spam@lostfound.com>" => "John Doe", "<jd+spam@lostfound.com>"
        if (subject.TrySplitAtLast(" ", out var displayName, out var emailPart))
        {
            DisplayName = displayName;
            subject = emailPart;
        }

        // "<jd+spam@lostfound.com>" => "jd+spam", "lostfound.com"
        emailPart = subject.TrimStart('<').TrimEnd('>');
        if (emailPart.TrySplitAtFirst("@", out var identity, out var domain))
        {
            Domain = domain;
            // "jd+spam@lostfound.com" => "jd+spam", "jd", "spam"
            if (identity.TrySplitAtFirst("+", out var username, out var mailbox))
            {
                Username = username;
                Mailbox = mailbox;
            }
            else
            {
                Username = identity;
            }

            if (IsValid)
            {
                return true;
            }
        }
        CopyFrom(state);

        return false;
    }

    public static bool TryParse(string email, out EmailAddress result)
    {
        result = new EmailAddress();
        return result.TryParse(email);
    }
    public bool IsValid => Username.Length > 1
                   && Domain.Contains('.')
                   && Domain.StartsWithOneOf("abcdefghijklmnopqrstuvwxyz_", false)
                   && Domain.ContainsOnlyCharactersIn(DomainAllowableCharacters, false)
                   && !DisplayName.Contains('<')
                   && !DisplayName.Contains('>');


    public override string ToString()
    {
        var sb = new StringBuilder();
        var needsAngleBrackets = false;
        if (!string.IsNullOrWhiteSpace(DisplayName))
        {
            needsAngleBrackets = true;
            sb.Append(DisplayName);
            sb.Append(" ");
        }

        if (needsAngleBrackets) sb.Append("<");

        sb.Append(Username);
        if (!string.IsNullOrWhiteSpace(Mailbox))
        {
            sb.Append("+");
            sb.Append(Mailbox);
        }
        sb.Append("@");
        sb.Append(Domain);

        if (needsAngleBrackets) sb.Append(">");
        return sb.ToString();
    }
}

public class InvalidEmailAddressException : Exception
{
    private const string ErrorMessage = "Invalid EmailAddress";
    public string InvalidEmailAddress { get; }
    public InvalidEmailAddressException(string emailAddress) : base(ErrorMessage)
    {
        InvalidEmailAddress = emailAddress;
    }
    public InvalidEmailAddressException(string emailAddress, Exception innerException) : base(ErrorMessage, innerException)
    {
        InvalidEmailAddress = emailAddress;
    }
}
