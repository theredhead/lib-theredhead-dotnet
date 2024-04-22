using theredhead.common;

namespace theredhead.common.tests;

[TestFixture]
public class AlsoTests
{
    [Test]
    public void Also_Modifies_Instance()
    {
        var instance = new EmailAddress("john.doe@lost-found.com")
            .Also(o => o.Mailbox = "important");

        Assert.That(instance.Mailbox, Is.EqualTo("important"));
    }
}
