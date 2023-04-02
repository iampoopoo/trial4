namespace Glekcraft.GLFW.Tests;

using Mocks;

[TestClass]
public class LibGLFWTests {
    /// <summary>
    /// The mock native APIs.
    /// </summary>
    public MockNativeAPIs MockAPIs {
        get;
    } = new();

    [TestInitialize]
    public void BeforeEach() =>
        MockAPIs.Reset();

    [TestCleanup]
    public void AfterEach() {
        LibGLFW.Instance?.Dispose();
        LibGLFW.ClearLastError();
    }

    [TestMethod]
    [Description("Test the 'IsInitialized' static property.")]
    [Owner("G'lek Tarssza")]
    [TestCategory("Core")]
    public void TestIsInitialized() {
        LibGLFW.IsInitialized.Should().BeFalse();
        using var lib = LibGLFW.Initialize(MockAPIs);
        LibGLFW.IsInitialized.Should().BeTrue();
        lib.Dispose();
        LibGLFW.IsInitialized.Should().BeFalse();
    }

    [TestMethod]
    [Description("Test the 'LastErrorCode' static property.")]
    [Owner("G'lek Tarssza")]
    [TestCategory("Core")]
    public void TestLastErrorCode() {
        LibGLFW.LastErrorCode.Should().BeNull();
        using var lib = LibGLFW.Initialize(MockAPIs);
        LibGLFW.LastErrorCode.Should().BeNull();
        MockAPIs.SetErrorCallbackInput?.Invoke(ErrorCode.PlatformError, "A test error.");
        LibGLFW.LastErrorCode.Should().Be(ErrorCode.PlatformError);
    }

    [TestMethod]
    [Description("Test the 'LastErrorDescription' static property.")]
    [Owner("G'lek Tarssza")]
    [TestCategory("Core")]
    public void TestLastErrorDescription() {
        LibGLFW.LastErrorDescription.Should().BeNull();
        using var lib = LibGLFW.Initialize(MockAPIs);
        LibGLFW.LastErrorDescription.Should().BeNull();
        MockAPIs.SetErrorCallbackInput?.Invoke(ErrorCode.PlatformError, "A test error.");
        LibGLFW.LastErrorDescription.Should().Be("A test error.");
    }

    [TestMethod]
    [Description("Test the 'Initialize' static method.")]
    [Owner("G'lek Tarssza")]
    [TestCategory("Core")]
    public void TestInitialize() {
        LibGLFW.IsInitialized.Should().BeFalse();
        using var lib = LibGLFW.Initialize(MockAPIs);
        LibGLFW.IsInitialized.Should().BeTrue();
        lib.Dispose();
        LibGLFW.IsInitialized.Should().BeFalse();
        MockAPIs.InitResult = false;
        FluentActions.Invoking(() => LibGLFW.Initialize(MockAPIs))
            .Should().Throw<NativeException>();
    }

    [TestMethod]
    [Description("Test the 'ClearLastError' static method.")]
    [Owner("G'lek Tarssza")]
    [TestCategory("Core")]
    public void TestClearLastError() {
        LibGLFW.IsInitialized.Should().BeFalse();
        using var lib = LibGLFW.Initialize(MockAPIs);
        LibGLFW.IsInitialized.Should().BeTrue();
        lib.Dispose();
        LibGLFW.IsInitialized.Should().BeFalse();
        MockAPIs.InitResult = false;
        FluentActions.Invoking(() => LibGLFW.Initialize(MockAPIs))
            .Should().Throw<NativeException>();
    }
}
