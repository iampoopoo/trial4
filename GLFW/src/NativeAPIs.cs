namespace Glekcraft.GLFW;

using System.Runtime.InteropServices;

internal static partial class NativeAPIs {
    #region Public Constants

    /// <summary>
    /// The name of the dynamic library to load entry points from.
    /// </summary>
    public const string DLL_NAME = "glfw";

    #endregion

    #region Public Native APIs

    [LibraryImport(DLL_NAME)]
    private static partial void glfwGetVersion(IntPtr major, IntPtr minor, IntPtr rev);

    [LibraryImport(DLL_NAME)]
    private static partial IntPtr glfwGetVersionString();

    #endregion

    #region Public Static Methods

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

    public static string GetVersionString() {
        var ptr = glfwGetVersionString();
        var version = Marshal.PtrToStringUTF8(ptr);
        if (version == null) {
            throw new InvalidOperationException("Failed to get version string");
        }
        return version;
    }

    public static bool TryGetVersionString(out string version) {
        var ptr = glfwGetVersionString();
        var str = Marshal.PtrToStringUTF8(ptr);
        if (str == null) {
            version = "";
            return false;
        }
        version = str;
        return true;
    }

    #endregion
}
