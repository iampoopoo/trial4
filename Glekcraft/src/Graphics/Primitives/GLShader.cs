namespace Glekcraft.Graphics.Primitives;

using System.Collections.Generic;
using System.Text;

using Silk.NET.OpenGL;

using Glekcraft.Graphics.Primitives.Exceptions;

public class GLShader : GLObject {
    #region Public Properties

    /// <summary>
    /// The type of OpenGL object wrapped by this instance.
    /// </summary>
    public override GLObjectType GLType =>
        GLObjectType.Shader;

    /// <summary>
    /// The type of OpenGL shader wrapped by this instance.
    /// </summary>
    public ShaderType Type { get; }

    /// <summary>
    /// The source code currently uploaded to this instance.
    /// </summary>
    public string? Source {
        get {
            if (!IsValid) {
                return null;
            }
            Context.GetShader(ID, ShaderParameterName.ShaderSourceLength, out var len);
            var src = new Span<byte>(new byte[len]);
            Context.GetShaderSource(ID, out _, src);
            return Encoding.UTF8.GetString(src);
        }
    }

    /// <summary>
    /// The information log from the last attempt to compile this instance.
    /// </summary>
    public string? InfoLog {
        get {
            if (!IsValid) {
                return null;
            }
            return Context.GetShaderInfoLog(ID);
        }
    }

    /// <summary>
    /// Whether this instance has been compiled.
    /// </summary>
    public bool IsCompiled {
        get {
            if (!IsValid) {
                return false;
            }
            Context.GetShader(ID, ShaderParameterName.CompileStatus, out var status);
            return status == 1;
        }
    }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="context">
    /// The OpenGL rendering context this instance will belong to.
    /// </param>
    /// <param name="type">
    /// The type of OpenGL shader to wrap in this instance.
    /// </param>
    /// <exception cref="GLObjectCreationFailedException">
    /// Thrown if the native OpenGL object could not be created.
    /// </exception>
    public GLShader(GL context, ShaderType type) : base(context) {
        ID = Context.CreateShader(type);
        if (!IsValid) {
            throw new GLObjectCreationFailedException(GLType);
        }
        Type = type;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Upload source code into the instance.
    /// </summary>
    /// <param name="source">
    /// The source code to upload.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not valid.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if an OpenGL operation failed.
    /// </exception>
    public void UploadSource(string source) {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShader));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        Context.ShaderSource(ID, source);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
    }

    /// <summary>
    /// Upload source code into the instance.
    /// </summary>
    /// <param name="source">
    /// The source code to upload.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not valid.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if an OpenGL operation failed.
    /// </exception>
    public void UploadSource(IEnumerable<string> sources) {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShader));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        Context.ShaderSource(ID, string.Join('\n', sources));
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
    }

    /// <summary>
    /// Compile the instance.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not valid.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if an OpenGL operation failed.
    /// </exception>
    /// <exception cref="GLShaderCompilationFailedException">
    /// Thrown if the shader failed to compile.
    /// </exception>
    public void Compile() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShader));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        Context.CompileShader(ID);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        if (!IsCompiled) {
            throw new GLShaderCompilationFailedException(InfoLog);
        }
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
    protected override void Dispose(bool managed) {
        if (managed && IsValid) {
            Context.DeleteShader(ID);
        }
        base.Dispose(managed);
    }

    #endregion
}
