namespace theredhead.text.tests;

public class StringComparisonAdditionsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_IsEquivalentToIgnoringWhitespace()
    {
        var intro = "Once upon a time in a galaxy far, far away…";
        var alternative = "Once upon a time\n\tin a galaxy far, far away…";

        Assert.That(intro.IsEquivalentToIgnoringWhitespace(alternative), Is.True);
    }
}
