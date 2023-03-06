namespace Glekcraft.GLFW;

using System.Runtime.InteropServices;

/// <summary>
/// A class which provides direct access to the GLFW APIs.
/// </summary>
internal static partial class NativeAPIs {
    #region Public Constants

    /// <summary>
    /// The name of the native library to access entry points from.
    /// </summary>
    public const string DLL_NAME = "glfw";

    #endregion

    #region Public Delegates

    public delegate void ErrorCallback(int error, IntPtr description);

    #endregion

    #region Internal Static Native APIs

    [LibraryImport(DLL_NAME)]
    private static partial void glfwGetVersion(IntPtr major, IntPtr minor, IntPtr rev);

    [LibraryImport(DLL_NAME)]
    private static partial IntPtr glfwGetVersionString();

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.I4)]
    private static partial bool glfwInit();

    [LibraryImport(DLL_NAME)]
    private static partial void glfwTerminate();

    [LibraryImport(DLL_NAME)]
    private static partial int glfwGetError(IntPtr description);

    [LibraryImport(DLL_NAME)]
    private static partial ErrorCallback? glfwSetErrorCallback(ErrorCallback? callback);

    [LibraryImport(DLL_NAME)]
    private static partial void glfwSwapBuffers();

    #endregion

    #region Public Static Native APIs

    public static Version GetVersion() {
        var majorPtr = Marshal.AllocHGlobal(sizeof(int));
        var minorPtr = Marshal.AllocHGlobal(sizeof(int));
        var revPtr = Marshal.AllocHGlobal(sizeof(int));

        glfwGetVersion(majorPtr, minorPtr, revPtr);

        var major = Marshal.ReadInt32(majorPtr);
        var minor = Marshal.ReadInt32(minorPtr);
        var rev = Marshal.ReadInt32(revPtr);

        Marshal.FreeHGlobal(majorPtr);
        Marshal.FreeHGlobal(minorPtr);
        Marshal.FreeHGlobal(revPtr);

        return new(major, minor, rev);
    }

    public static string? GetVersionString() {
        var ptr = glfwGetVersionString();
        return Marshal.PtrToStringUTF8(ptr);
    }

    public static bool Init() => glfwInit();

    public static void Terminate() => glfwTerminate();

    public static int GetError(out string? description) {
        var descriptionPtr = Marshal.AllocHGlobal(IntPtr.Size);

        var error = glfwGetError(descriptionPtr);

        var descriptionPtr2 = Marshal.ReadIntPtr(descriptionPtr);
        description = Marshal.PtrToStringUTF8(descriptionPtr2);

        Marshal.FreeHGlobal(descriptionPtr);

        return error;
    }

    public static ErrorCallback? SetErrorCallback(ErrorCallback? callback) => glfwSetErrorCallback(callback);

    public static void SwapBuffers() => glfwSwapBuffers();

    #endregion
}
