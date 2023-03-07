namespace Glekcraft.GLFW;

using System.Runtime.InteropServices;

public sealed class LibGLFW : IDisposable {
    #region Private Static Fields

    internal static LibGLFW? s_instance;

    #endregion

    #region Public Static Methods

    public static Version NativeVersion =>
        NativeAPIs.GetVersion();

    public static string? NativeVersionString =>
        NativeAPIs.GetVersionString();

    public static int? LastErrorCode {
        get;
        private set;
    }

    public static string? LastErrorDescription {
        get;
        private set;
    }

    public static bool IsInitialized =>
        s_instance != null;

    public static LibGLFW Initialize() {
        if (s_instance == null) {
            if (!NativeAPIs.Init()) {
                throw new GLFWException("Failed to initialize GLFW");
            }
            s_instance = new();
        }
        return s_instance;
    }

    #endregion

    #region Public Static Methods

    public static void ClearLastError() {
        LastErrorCode = null;
        LastErrorDescription = null;
    }

    #endregion

    #region Private Static Methods

    private static void ErrorCallback(int error, IntPtr description) {
        LastErrorCode = error;
        LastErrorDescription = Marshal.PtrToStringUTF8(description);
    }

    #endregion

    #region Public Properties

    public bool IsCurrentInstance =>
        s_instance == this;

    public bool IsDisposed {
        get;
        private set;
    }

    #endregion

    #region Constructors/Finalizer

    static LibGLFW() =>
        NativeAPIs.SetErrorCallback(ErrorCallback);

    private LibGLFW() {
        //-- Does nothing
    }

    ~LibGLFW() =>
        Dispose(false);

    #endregion

    #region Public Methods

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Methods

    private void Dispose(bool managed) {
        if (IsDisposed) {
            return;
        }
        if (managed) {
            // TODO
        }
        if (IsCurrentInstance) {
            NativeAPIs.Terminate();
            s_instance = null;
        }
        IsDisposed = true;
    }

    #endregion
}
