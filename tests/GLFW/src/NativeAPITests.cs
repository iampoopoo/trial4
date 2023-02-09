namespace Glekcraft.GLFW.Tests;

[TestClass]
public class NativeAPITests {
    [TestMethod]
    public void TestGetVersion() {
        NativeAPIs.glfwGetVersion(out var major, out var minor, out var rev);
        major.Should().BeGreaterThanOrEqualTo(3);
        minor.Should().BeGreaterThanOrEqualTo(0);
        rev.Should().BeGreaterThanOrEqualTo(0);
    }
}
