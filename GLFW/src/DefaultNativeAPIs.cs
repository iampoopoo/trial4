namespace Glekcraft.GLFW;

using System.Runtime.InteropServices;

/// <summary>
/// A class that provides access to native GLFW APIs.
/// </summary>
internal partial class DefaultNativeAPIs : INativeAPIs {
    #region Public Constants

    /// <summary>
    /// The name of the dynamic library to load the native APIs from.
    /// </summary>
    public const string DLL_NAME = "glfw";

    #endregion

    #region Private Static Methods

    [LibraryImport(DLL_NAME)]
    private static partial void glfwGetVersion(out int major, out int minor, out int rev);

    [LibraryImport(DLL_NAME)]
    private static partial IntPtr glfwGetVersionString();

    [LibraryImport(DLL_NAME)]
    private static partial void glfwInitHint(int hint, int value);

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool glfwInit();

    [LibraryImport(DLL_NAME)]
    private static partial void glfwTerminate();

    [LibraryImport(DLL_NAME)]
    private static partial int glfwGetError(IntPtr description);

    [LibraryImport(DLL_NAME)]
    private static partial INativeAPIs.ErrorCallback? glfwSetErrorCallback(INativeAPIs.ErrorCallback? callback);

    #endregion

    #region Public Methods

    public Version GetVersion() {
        glfwGetVersion(out var major, out var minor, out var rev);
        return new(major, minor, rev);
    }

    public string GetVersionString() =>
        Marshal.PtrToStringUTF8(glfwGetVersionString())!;

    public void InitHint(int hint, int value) =>
            glfwInitHint(hint, value);

    public void InitHint(int hint, bool value) =>
        glfwInitHint(hint, value ? 1 : 0);

    public bool Init() =>
        glfwInit();

    public void Terminate() =>
        glfwTerminate();

    public int GetError(out string? description) {
        var descriptionPtr = Marshal.AllocHGlobal(IntPtr.Size);
        var error = glfwGetError(descriptionPtr);
        description = Marshal.PtrToStringUTF8(Marshal.ReadIntPtr(descriptionPtr));
        Marshal.FreeHGlobal(descriptionPtr);
        return error;
    }

    public INativeAPIs.ErrorCallback? SetErrorCallback(INativeAPIs.ErrorCallback? callback) =>
        glfwSetErrorCallback(callback);

    #endregion
}
