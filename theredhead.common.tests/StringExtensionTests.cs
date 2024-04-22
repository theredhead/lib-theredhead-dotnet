namespace theredhead.common.tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase("Hello, World!", ", ", "Hello", "World!", true)]
    [TestCase("!***", "!", "", "***", true)]
    [TestCase("*!**", "!", "*", "**", true)]
    [TestCase("**!*", "!", "**", "*", true)]
    [TestCase("***!", "!", "***", "", true)]
    [TestCase("****", "!", "", "", false)]
    [TestCase("!***!***", "!", "", "***!***", true)]
    [TestCase("*!**!***", "!", "*", "**!***", true)]
    [TestCase("**!*!***", "!", "**", "*!***", true)]
    [TestCase("***!!***", "!", "***", "!***", true)]
    [TestCase("****!***", "!", "****", "***", true)]
    public void TrySplitAtFirst_WithKnownInput_ProducesExpectedOutput(string subject, string splitter, string expectedLeft, string expectedRight, bool expectedSuccess)
    {
        var success = subject.TrySplitAtFirst(splitter, out var left, out var right);
        Assert.That(success, Is.EqualTo(expectedSuccess));
        Assert.That(left, Is.EqualTo(expectedLeft));
        Assert.That(right, Is.EqualTo((expectedRight)));
    }

    [TestCase("Hello, World!", ", ", "Hello", "World!", true)]
    [TestCase("!***", "!", "", "***", true)]
    [TestCase("*!**", "!", "*", "**", true)]
    [TestCase("**!*", "!", "**", "*", true)]
    [TestCase("***!", "!", "***", "", true)]
    [TestCase("****", "!", "", "", false)]
    [TestCase("!***!***", "!", "!***", "***", true)]
    [TestCase("!****!**", "!", "!****", "**", true)]
    [TestCase("!*****!*", "!", "!*****", "*", true)]
    [TestCase("!******!", "!", "!******", "", true)]
    [TestCase("****!***", "!", "****", "***", true)]
    public void TrySplitAtLast_WithKnownInput_ProducesExpectedOutput(string subject, string splitter, string expectedLeft, string expectedRight, bool expectedSuccess)
    {
        var success = subject.TrySplitAtLast(splitter, out var left, out var right);
        Assert.That(success, Is.EqualTo(expectedSuccess));
        Assert.That(left, Is.EqualTo(expectedLeft));
        Assert.That(right, Is.EqualTo((expectedRight)));
    } 

    public void IStringRepresentable_WhenGivenValidString_DoesNotThrow(Type type, string value)
    {
        Assert.That(type.GetInterfaces().Contains(typeof(IStringRepresentable)));

        Assert.DoesNotThrow(() =>
        {
            var _ = StringExtensions.As(value, type);
        });
    }
}
