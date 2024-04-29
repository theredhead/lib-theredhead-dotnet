namespace theredhead.aspect.tests;


[Copyable]
public partial class CopyableTestHelper {
    public int Id { get; set; } = 0;
    public string Foo { get; set; } = "Foo";
    public string Bar { get; set; } = "Bar";
}

[TestFixture]
public class CopyAspectTests
{
    [Test]
    public void EnsureThatTheCopyMethodIsPresent()
    {
        var info = typeof(CopyableTestHelper);
        var method = info.GetMethod("Copy");
        Assert.That(method, Is.Not.Null);
    }

    [Test]
    public void EnsureThatTheCopyMethodWorks()
    {
        var original = new CopyableTestHelper();
        var copy = original.Copy();

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.Foo, Is.EqualTo(original.Foo));
        Assert.That(copy.Bar, Is.EqualTo(original.Bar));
    }

    [Test]
    public void EnsureThatTheCopyMethodWorks_WhenGivenAnOverride()
    {
        var original = new CopyableTestHelper();
        var copy = original.Copy(bar: "different");

        Assert.That(copy.Id, Is.EqualTo(original.Id));
        Assert.That(copy.Foo, Is.EqualTo(original.Foo));
        Assert.That(copy.Bar, Is.Not.EqualTo(original.Bar));
        Assert.That(copy.Bar, Is.EqualTo("different"));
    }
}
