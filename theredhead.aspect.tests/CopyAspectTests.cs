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
    public void Test1()
    {
        var info = typeof(CopyableTestHelper);
        var method = info.GetMethod("Copy");
        Assert.That(method, Is.Not.Null);
    }
}