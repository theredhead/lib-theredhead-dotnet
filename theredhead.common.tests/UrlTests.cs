using theredhead.core;

namespace theredhead.common.tests;

[TestFixture]
public class UrlTests
{
    [Test]
    [TestCase("query=string")]
    [TestCase("roses=red&violets=blue")]
    public void QueryString_WhenGivenValidString_DoesNotThrow(string queryString)
    {
        Assert.DoesNotThrow(() =>
        {
            var _ = new Url.QueryString(queryString);
        });
    }
}
