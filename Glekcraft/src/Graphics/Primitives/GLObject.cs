namespace Glekcraft.Graphics.Primitives;

using Silk.NET.OpenGL;

/// <summary>
/// An abstract wrapper around an OpenGL object.
/// </summary>
public abstract class GLObject : IDisposable {
    #region Public Properties

    /// <summary>
    /// The OpenGL rendering context this instance belongs to.
    /// </summary>
    public GL Context { get; }

    /// <summary>
    /// The OpenGL ID of the object wrapped by this instance.
    /// </summary>
    public uint ID { get; private set; }

    /// <summary>
    /// Whether this instance has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Whether this instance is valid.
    /// </summary>
    public bool IsValid =>
        !IsDisposed && ID != 0;

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="context">
    /// The OpenGL rendering context this instance will belong to.
    /// </param>
    protected GLObject(GL context) {
        Context = context;
        ID = 0;
    }

    /// <summary>
    /// The finalizer.
    /// </summary>
    ~GLObject() =>
        Dispose(false);

    #endregion

    #region Public Methods

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    /// <seealso cref="IsDisposed" />
    /// <seealso cref="IsValid" />
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Dispose of this instance.
    /// </summary>
    /// <param name="managed">
    /// Whether this method is being called from managed code or from unmanaged
    /// code (e.g. the garbage collector).
    /// </param>
    /// <seealso cref="IsDisposed" />
    /// <seealso cref="IsValid" />
    /// <seealso cref="Dispose" />
    protected virtual void Dispose(bool managed) {
        ID = 0;
        IsDisposed = true;
    }

    #endregion
}
