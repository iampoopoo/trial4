namespace Glekcraft.Graphics.Primitives;

using System.Collections.Generic;

using Silk.NET.OpenGL;

using Glekcraft.Graphics.Primitives.Exceptions;

/// <summary>
/// An OpenGL shader program resource.
/// </summary>
public class GLShaderProgram : GLObject {
    #region Private Fields

    /// <summary>
    /// The list of shaders that are attached to this instance.
    /// </summary>
    private readonly IList<GLShader> attachedShaders;

    #endregion

    #region Public Properties

    /// <summary>
    /// The type of OpenGL object wrapped by this instance.
    /// </summary>
    public override GLObjectType GLType =>
        GLObjectType.ShaderProgram;

    /// <summary>
    /// The list of shaders that are attached to this instance.
    /// </summary>
    public GLShader[] AttachedShaders {
        get {
            var shaders = new GLShader[attachedShaders.Count];
            attachedShaders.CopyTo(shaders, 0);
            return shaders;
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
            return Context.GetProgramInfoLog(ID);
        }
    }

    /// <summary>
    /// Whether this instance has been linked.
    /// </summary>
    public bool IsLinked {
        get {
            if (!IsValid) {
                return false;
            }
            Context.GetProgram(ID, ProgramPropertyARB.LinkStatus, out var status);
            return status == 1;
        }
    }

    /// <summary>
    /// Whether this instance is the currently active shader program.
    /// </summary>
    public bool IsActive {
        get {
            if (!IsValid) {
                return false;
            }
            Context.GetInteger(GetPName.CurrentProgram, out var result);
            return result == ID;
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
    /// <exception cref="GLObjectCreationFailedException">
    /// Thrown if the OpenGL object could not be created.
    /// </exception>
    public GLShaderProgram(GL context) : base(context) {
        ID = Context.CreateProgram();
        if (!IsValid) {
            throw new GLObjectCreationFailedException(GLType, (ErrorCode)Context.GetError());
        }
        attachedShaders = new List<GLShader>();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Check whether this instance has the specified shader attached.
    /// </summary>
    /// <param name="shader">
    /// The shader to check for.
    /// </param>
    /// <returns>
    /// Whether this instance has the specified shader attached.
    /// </returns>
    public bool HasAttachedShader(GLShader shader) =>
        attachedShaders.Contains(shader);

    /// <summary>
    /// Attach the specified shader to this instance.
    /// </summary>
    /// <param name="shader">
    /// The shader to attach.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if the shader has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not valid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the shader is not valid.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if an OpenGL operation failed.
    /// </exception>
    public void AttachShader(GLShader shader) {
        if (HasAttachedShader(shader)) {
            return;
        }
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShaderProgram));
        }
        if (shader.IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShader));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        if (!shader.IsValid) {
            throw new InvalidOperationException();
        }
        Context.AttachShader(ID, shader.ID);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        attachedShaders.Add(shader);
    }

    /// <summary>
    /// Detach the specified shader from this instance.
    /// </summary>
    /// <param name="shader">
    /// The shader to detach.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if the shader has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this instance is not valid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the shader is not valid.
    /// </exception>
    /// <exception cref="GLOperationException">
    /// Thrown if an OpenGL operation failed.
    /// </exception>
    public void DetachShader(GLShader shader) {
        if (!HasAttachedShader(shader)) {
            return;
        }
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShaderProgram));
        }
        if (shader.IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShader));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        if (!shader.IsValid) {
            throw new InvalidOperationException();
        }
        Context.DetachShader(ID, shader.ID);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        _ = attachedShaders.Remove(shader);
    }

    /// <summary>
    /// Link this instance.
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
    public void Link() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShaderProgram));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        Context.LinkProgram(ID);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
        if (!IsLinked) {
            throw new GLShaderProgramLinkingFailedException(InfoLog);
        }
    }

    /// <summary>
    /// Activate this instance.
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
    public void Activate() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShaderProgram));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        if (IsActive) {
            return;
        }
        Context.UseProgram(ID);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
        }
    }

    /// <summary>
    /// Deactivate this instance.
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
    public void Deactivate() {
        if (IsDisposed) {
            throw new ObjectDisposedException(nameof(GLShaderProgram));
        }
        if (!IsValid) {
            throw new InvalidOperationException();
        }
        if (!IsActive) {
            return;
        }
        Context.UseProgram(0);
        var err = (ErrorCode)Context.GetError();
        if (err != ErrorCode.NoError) {
            throw new GLOperationException(err);
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
        if (IsValid) {
            Context.DeleteProgram(ID);
        }
        base.Dispose(managed);
    }

    #endregion
}
