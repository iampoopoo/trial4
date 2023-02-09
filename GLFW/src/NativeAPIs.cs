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
    public static partial void glfwGetVersion(out int major, out int minor, out int rev);

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    public static partial string glfwGetVersionString();

    #endregion
}
