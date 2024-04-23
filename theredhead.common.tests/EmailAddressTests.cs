using theredhead.common;

namespace theredhead.common.tests;

[TestFixture]
public class EmailAddressTests
{
    public static IEnumerable<string> KnownGoodEmailAddresses()
    {
        yield return "John Doe <john-doe+important@lost-found.com>";
        yield return "John Doe <john-doe@lost-found.com>";

        yield return "john-doe+important@lost-found.com";
        yield return "john-doe@lost-found.com";
    }

    public static IEnumerable<string> KnownGoodEmailAddressesBadlyPresented()
    {
        yield return "<john-doe+important@lost-found.com>";
        yield return "<john-doe@lost-found.com>";
    }

    public static IEnumerable<string> ObviouslyNotEmailAddresses()
    {
        // this null is on purpose, you nevr know what happens at runtime in the wild
#pragma warning disable CS8603 // Possible null reference return.
        yield return null;
#pragma warning restore CS8603 // Possible null reference return.
        yield return "";
        yield return "The quick brown fox, jumps over the lazy dog.";
    }

    [Test]
    [TestCaseSource(nameof(KnownGoodEmailAddresses))]
    public void EmailAddressConstructor_WithKnownGoodEmailAddresses_DoesNotThrow(string email)
    {
        Assert.DoesNotThrow(() =>
        {
            var _ = new EmailAddress(email);
        });
    }
    [Test]
    [TestCaseSource(nameof(ObviouslyNotEmailAddresses))]
    public void EmailAddressConstructor_WithNonEmailAddressStrings_ThrowsInvalidEmailAddressException(string email)
    {
        Assert.Throws<InvalidEmailAddressException>(() =>
        {
            var _ = new EmailAddress(email);
        });
    }

    [Test]
    [TestCaseSource(nameof(KnownGoodEmailAddresses))]
    public void EmailAddressToString_WithKnownGoodEmailAddresses_ReturnsOriginalString(string email)
    {
        var emailAddress = new EmailAddress(email);
        Assert.That(emailAddress.ToString(), Is.EqualTo(email));
    }

    [Test]
    [TestCaseSource(nameof(KnownGoodEmailAddresses))]
    public void EmailAddress_Is_StringRepresentble (string email)
    {
        Assert.DoesNotThrow(() =>
        {
            var result = email.As<EmailAddress>();
            Assert.That(result, Is.Not.Null);
        });
    }
}
