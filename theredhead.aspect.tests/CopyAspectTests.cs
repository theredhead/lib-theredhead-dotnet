namespace theredhead.aspect.tests;


[Copyable]
public partial class CopyableTestHelper {

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