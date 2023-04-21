namespace Glekcraft.Graphics.Primitives;

using System.Runtime.InteropServices;

using Silk.NET.OpenGL;

using Glekcraft.Graphics.Primitives.Exceptions;

/// <summary>
/// An OpenGL buffer resource.
/// </summary>
public class GLBuffer : IDisposable {
    #region Public Properties

    /// <summary>
    /// The OpenGL rendering context that owns this instance.
    /// </summary>
    public GL Context { get; }

    /// <summary>
    /// The OpenGL ID of the wrapped resource.
    /// </summary>
    public uint ID { get; }

    /// <summary>
    /// The target the instance will bind to.
    /// </summary>
    public BufferTargetARB Target { get; }

    /// <summary>
    /// The amount of VRAM allocated to this instance, in bytes.
    /// </summary>
    public uint? SizeBytes {
        get;
        private set;
    }

    /// <summary>
    /// The usage hint for this instance.
    /// </summary>
    public BufferUsageARB? UsageHint {
        get;
        private set;
    }

    /// <summary>
    /// Whether this instance is bound to its target.
    /// </summary>
    public bool IsBound {
        get {
            if (!IsValid) {
                return false;
            }
#pragma warning disable IDE0072
            // TODO: Populate the rest of this switch
            var pname = Target switch {
                BufferTargetARB.ArrayBuffer => GetPName.ArrayBufferBinding,
                BufferTargetARB.ElementArrayBuffer => GetPName.ElementArrayBufferBinding,
                _ => throw new NotImplementedException(),
            };
#pragma warning restore IDE0072
            return Context.GetInteger(pname) == ID;
        }
    }

    /// <summary>
    /// Whether this instance wraps a valid resource.
    /// </summary>
    public bool IsValid =>
        !IsDisposed && ID != 0;

    /// <summary>
    /// Whether this instance has been disposed.
    /// </summary>
    public bool IsDisposed {
        get;
        private set;
    }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="context">
    /// The OpenGL rendering context that will own this instance.
    /// </param>
    /// <param name="target">
    /// The target this instance will bind to.
    /// </param>
    /// <exception cref="GLObjectCreationFailedException">
    /// Thrown if the OpenGL rendering context fails to create the resource.
    /// </exception>
    public GLBuffer(GL context, BufferTargetARB target) {
        Context = context;
        Target = target;
        ID = Context.GenBuffer();
        if (!IsValid) {
            throw new GLObjectCreationFailedException(GLObjectType.Buffer, (ErrorCode)Context.GetError());
        }
    }

    /// <summary>
    /// The finalizer.
    /// </summary>
    ~GLBuffer() =>
        Dispose(false);

    #endregion

    #region Public Methods

    /// <summary>
    /// Bind this instance to its target.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance does not wrap a valid resource.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if the OpenGL rendering context fails to bind the resource.
    /// </exception>
    public void Bind() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLBuffer));
        }
        if (!IsValid) {
            throw new InvalidOperationException("Invalid OpenGL buffer object");
        }
        Context.BindBuffer(Target, ID);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
    }

    /// <summary>
    /// Allocate space in VRAM for this instance.
    /// </summary>
    /// <param name="sizeBytes">
    /// The amount of space to allocate, in bytes.
    /// </param>
    /// <param name="usageHint">
    /// The usage hint to set for this instance.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance does not wrap a valid resource.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not bound to its target.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if the OpenGL rendering context fails to bind the resource.
    /// </exception>
    public void Allocate(uint sizeBytes, BufferUsageARB usageHint = BufferUsageARB.StaticDraw) {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLBuffer));
        }
        if (!IsValid) {
            throw new InvalidOperationException("Invalid OpenGL buffer object");
        }
        if (!IsBound) {
            throw new InvalidOperationException("OpenGL buffer object not bound");
        }
        Context.BufferData(Target, sizeBytes, 0, usageHint);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        SizeBytes = sizeBytes;
        UsageHint = usageHint;
    }

    /// <summary>
    /// Upload data into this instance.
    /// </summary>
    /// <param name="data">
    /// The data to upload.
    /// </param>
    /// <param name="usageHint">
    /// The usage hint to set for this instance.
    /// </param>
    /// <typeparam name="T0">
    /// The type of data being uploaded.
    /// </typeparam>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance does not wrap a valid resource.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not bound to its target.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if the OpenGL rendering context fails to bind the resource.
    /// </exception>
    public void UploadData<T0>(in T0[] data, BufferUsageARB usageHint = BufferUsageARB.StaticDraw) where T0 : unmanaged {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLBuffer));
        }
        if (!IsValid) {
            throw new InvalidOperationException("Invalid OpenGL buffer object");
        }
        if (!IsBound) {
            throw new InvalidOperationException("OpenGL buffer object not bound");
        }
        Context.BufferData<T0>(Target, data, usageHint);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        SizeBytes = (uint)(data.Length * Marshal.SizeOf<T0>());
        UsageHint = usageHint;
    }

    /// <summary>
    /// Upload data into this instance.
    /// </summary>
    /// <param name="data">
    /// The data to upload.
    /// </param>
    /// <param name="usageHint">
    /// The usage hint to set for this instance.
    /// </param>
    /// <typeparam name="T0">
    /// The type of data being uploaded.
    /// </typeparam>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance does not wrap a valid resource.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not bound to its target.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if the OpenGL rendering context fails to bind the resource.
    /// </exception>
    public void UploadData<T0>(ReadOnlySpan<T0> data, BufferUsageARB usageHint = BufferUsageARB.StaticDraw) where T0 : unmanaged {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLBuffer));
        }
        if (!IsValid) {
            throw new InvalidOperationException("Invalid OpenGL buffer object");
        }
        if (!IsBound) {
            throw new InvalidOperationException("OpenGL buffer object not bound");
        }
        Context.BufferData(Target, data, usageHint);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        SizeBytes = (uint)(data.Length * Marshal.SizeOf<T0>());
        UsageHint = usageHint;
    }

    /// <summary>
    /// Upload data into this instance at a given offset into the existing
    /// allocation.
    /// </summary>
    /// <param name="data">
    /// The data to upload.
    /// </param>
    /// <param name="offset">
    /// The offset into the existing allocation to upload the data to.
    /// </param>
    /// <typeparam name="T0">
    /// The type of data being uploaded.
    /// </typeparam>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance does not wrap a valid resource.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not bound to its target.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="offset"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="offset" /> plus the size of the
    /// <paramref name="data" /> is greater than or equal to the size of the
    /// existing allocation.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if the OpenGL rendering context fails to bind the resource.
    /// </exception>
    public void UploadSubData<T0>(in T0[] data, int offset = 0) where T0 : unmanaged {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLBuffer));
        }
        if (!IsValid) {
            throw new InvalidOperationException("Invalid OpenGL buffer object");
        }
        if (!IsBound) {
            throw new InvalidOperationException("OpenGL buffer object not bound");
        }
        if (offset < 0) {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        if (offset + (data.Length * Marshal.SizeOf<T0>()) >= SizeBytes) {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        Context.BufferSubData<T0>(Target, offset, data);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
    }

    /// <summary>
    /// Upload data into this instance at a given offset into the existing
    /// allocation.
    /// </summary>
    /// <param name="data">
    /// The data to upload.
    /// </param>
    /// <param name="offset">
    /// The offset into the existing allocation to upload the data to.
    /// </param>
    /// <typeparam name="T0">
    /// The type of data being uploaded.
    /// </typeparam>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance does not wrap a valid resource.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not bound to its target.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="offset"/> is less than zero.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="offset" /> plus the size of the
    /// <paramref name="data" /> is greater than or equal to the size of the
    /// existing allocation.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if the OpenGL rendering context fails to bind the resource.
    /// </exception>
    public void UploadSubData<T0>(ReadOnlySpan<T0> data, int offset = 0) where T0 : unmanaged {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLBuffer));
        }
        if (!IsValid) {
            throw new InvalidOperationException("Invalid OpenGL buffer object");
        }
        if (!IsBound) {
            throw new InvalidOperationException("OpenGL buffer object not bound");
        }
        if (offset < 0) {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        if (offset + (data.Length * Marshal.SizeOf<T0>()) >= SizeBytes) {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }
        Context.BufferSubData(Target, offset, data);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
    }

    /// <summary>
    /// Dispose of this instance and its wrapped resource.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Dispose of this instance and its wrapped resource.
    /// </summary>
    /// <param name="managed">
    /// Whether this method is being called from managed code or from unmanaged
    /// code (e.g. the garbage collector).
    /// </param>
    private void Dispose(bool managed) {
        if (!IsValid) {
            return;
        }
        if (managed) {
            Context.DeleteBuffer(ID);
        }
        IsDisposed = true;
    }

    #endregion
}
