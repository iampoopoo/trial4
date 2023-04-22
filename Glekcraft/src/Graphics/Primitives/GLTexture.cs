namespace Glekcraft.Graphics.Primitives;

using System.Collections.Generic;

using Silk.NET.OpenGL;

using Glekcraft.Graphics.Primitives.Exceptions;

/// <summary>
/// An OpenGL texture resource.
/// </summary>
public class GLTexture : GLObject {
    #region Public Properties

    /// <summary>
    /// The type of OpenGL object wrapped by this instance.
    /// </summary>
    public override GLObjectType GLType =>
        GLObjectType.Texture;

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
    public GLTexture(GL context) : base(context) {
        ID = Context.GenTexture();
        if (!IsValid) {
            throw new GLObjectCreationFailedException(GLType, (ErrorCode)Context.GetError());
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
            Context.DeleteTexture(ID);
        }
        base.Dispose(managed);
    }

    #endregion
}
