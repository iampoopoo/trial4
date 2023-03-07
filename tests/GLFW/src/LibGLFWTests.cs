namespace Glekcraft.GLFW.Tests;

[TestClass]
public class LibGLFWTests {
    [TestInitialize]
    public void BeforeEach() {
        LibGLFW.ClearLastError();
        LibGLFW.s_instance?.Dispose();
    }

    [TestMethod]
    public void TestInitialize() {
        using var lib = FluentActions.Invoking(LibGLFW.Initialize)
            .Should().NotThrow()
            .Which;
        LibGLFW.Initialize()
            .Should().BeSameAs(lib);
        lib.Dispose();
        LibGLFW.Initialize()
            .Should().NotBeSameAs(lib);
    }

    [TestMethod]
    public void TestIsInitialized() {
        LibGLFW.IsInitialized.Should().BeFalse();
        using var lib = LibGLFW.Initialize();
        LibGLFW.IsInitialized.Should().BeTrue();
    }

    [TestMethod]
    public void TestIsCurrentInstance() {
        using var lib = LibGLFW.Initialize();
        lib.IsCurrentInstance.Should().BeTrue();
        lib.Dispose();
        lib.IsCurrentInstance.Should().BeFalse();
    }

    [TestMethod]
    public void TestLastErrorCode() {
        LibGLFW.LastErrorCode.Should().BeNull();
        //-- Manually force an error
        NativeAPIs.SwapBuffers();
        LibGLFW.LastErrorCode.Should().NotBeNull()
            .And.BeGreaterThan(0);
    }

    [TestMethod]
    public void TestLastErrorDescription() {
        LibGLFW.LastErrorDescription.Should().BeNull();
        //-- Manually force an error
        NativeAPIs.SwapBuffers();
        LibGLFW.LastErrorDescription.Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public void TestDispose() {
        using var lib = LibGLFW.Initialize();
        FluentActions.Invoking(lib.Dispose)
            .Should().NotThrow();
        lib.IsCurrentInstance.Should().BeFalse();
        LibGLFW.IsInitialized.Should().BeFalse();
    }

    [TestMethod]
    public void TestNativeVersion() =>
        LibGLFW.NativeVersion.Should()
            .BeGreaterThanOrEqualTo(new(3, 0, 0));

    [TestMethod]
    public void TestNativeVersionString() =>
        LibGLFW.NativeVersionString.Should()
            .NotBeNullOrWhiteSpace()
            .And.MatchRegex(@"^3\.\d+\.\d+");
}
