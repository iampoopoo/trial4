namespace Glekcraft.GLFW.Tests;

[TestClass]
public class NativeAPITests {
    [TestMethod]
    public void TestGetVersion() =>
        NativeAPIs.GetVersion().Should().BeGreaterThanOrEqualTo(new(3, 0, 0));

    [TestMethod]
    public void TestGetVersionString() =>
        NativeAPIs.GetVersionString().Should().NotBeNullOrWhiteSpace().And.MatchRegex(@"3\.\d+\.\d+");

    [TestMethod]
    public void TestTryGetVersionString() {
        NativeAPIs.TryGetVersionString(out var version).Should().BeTrue();
        version.Should().NotBeNullOrWhiteSpace().And.MatchRegex(@"3\.\d+\.\d+");
    }
}
