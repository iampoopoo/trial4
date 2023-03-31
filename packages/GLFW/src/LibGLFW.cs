namespace Glekcraft.GLFW;

/// <summary>
/// The main entry point into the library.
/// </summary>
public sealed class LibGLFW : IDisposable {
    #region Internal Static Properties

    /// <summary>
    /// The instance which is currently managing the native library.
    /// </summary>
    internal static LibGLFW? Instance {
        get;
        private set;
    }

    #endregion

    #region Public Static Properties

    /// <summary>
    /// Whether the native library is initialized.
    /// </summary>
    public static bool IsInitialized =>
        Instance is not null;

    #endregion

    #region Public Static Methods

    /// <summary>
    /// Initialize the library.
    /// </summary>
    /// <returns>
    /// The existing instance if one is available, otherwise a new instance.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if the native library failed to initialize.
    /// </exception>
    public static LibGLFW Initialize(INativeAPIs? nativeAPIs = null) {
        Instance ??= new(nativeAPIs ?? new DefaultNativeAPIs());
        return Instance;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Whether this instance has been disposed.
    /// </summary>
    /// <seealso cref="Dispose"/>
    /// <seealso cref="Terminate"/>
    public bool IsDisposed {
        get;
        private set;
    }

    /// <summary>
    /// Whether this instance is the one currently managing the native library.
    /// </summary>
    public bool IsCurrentInstance =>
        Instance == this;

    /// <summary>
    /// The native APIs in use by this instance.
    /// </summary>
    public INativeAPIs NativeAPIs {
        get;
    }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <exception cref="Exception">
    /// Thrown if the native library failed to initialize.
    /// </exception>
    private LibGLFW(INativeAPIs nativeAPIs) {
        NativeAPIs = nativeAPIs;
        if (!NativeAPIs.Init()) {
            throw new Exception("Failed to initialize the native library");
        }
    }

    /// <summary>
    /// The finalizer.
    /// </summary>
    ~LibGLFW() =>
        Dispose(false);

    #endregion

    #region Public Methods

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    /// <seealso cref="Dispose"/>
    /// <seealso cref="IsDisposed"/>
    public void Terminate() =>
        Dispose();

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    /// <seealso cref="Terminate"/>
    /// <seealso cref="IsDisposed"/>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    /// <param name="managed">
    /// Whether this method is being called from managed code or from unmanaged
    /// code (e.g. the garbage collector)
    /// </param>
    /// <seealso cref="Dispose"/>
    /// <seealso cref="Terminate"/>
    /// <seealso cref="IsDisposed"/>
    private void Dispose(bool managed) {
        if (IsDisposed) {
            return;
        }
        if (IsCurrentInstance) {
            if (managed) {
                // TODO: Dispose of managed resources
            }
            NativeAPIs.Terminate();
            Instance = null;
        }
        // TODO: Dispose of own resources
        IsDisposed = true;
    }

    #endregion
}
