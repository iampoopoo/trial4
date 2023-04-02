namespace Glekcraft.GLFW;

using System.Runtime.Serialization;

/// <summary>
/// An exception for errors raised from the native library.
/// </summary>
[Serializable]
public class NativeException : Exception {
    #region Public Properties

    /// <summary>
    /// The error code raised by the native library.
    /// </summary>
    public ErrorCode? NativeErrorCode {
        get;
    }

    /// <summary>
    /// The error description raised by the native library.
    /// </summary>
    public string? NativeErrorDescription {
        get;
    }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    public NativeException() : base() {
        NativeErrorCode = LibGLFW.LastErrorCode;
        NativeErrorDescription = LibGLFW.LastErrorDescription;
    }

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="message">
    /// The message for the exception.
    /// </param>
    public NativeException(string message) : base(message) {
        NativeErrorCode = LibGLFW.LastErrorCode;
        NativeErrorDescription = LibGLFW.LastErrorDescription;
    }

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="message">
    /// The message for the exception.
    /// </param>
    /// <param name="inner">
    /// The inner exception.
    /// </param>
    public NativeException(string message, Exception inner) : base(message, inner) {
        NativeErrorCode = LibGLFW.LastErrorCode;
        NativeErrorDescription = LibGLFW.LastErrorDescription;
    }

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="info">
    /// The serialization information.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    protected NativeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        if (info.GetBoolean("HasNativeErrorCode")) {
            NativeErrorCode = (ErrorCode?)info.GetValue("NativeErrorCode", typeof(ErrorCode));
        }
        if (info.GetBoolean("HasNativeErrorDescription")) {
            NativeErrorDescription = info.GetString("NativeErrorDescription");
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Get the object data for serialization.
    /// </summary>
    /// <param name="info">
    /// The serialization information.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("HasNativeErrorCode", NativeErrorCode != null);
        if (NativeErrorCode != null) {
            info.AddValue("NativeErrorCode", NativeErrorCode);
        }
        info.AddValue("HasNativeErrorDescription", NativeErrorDescription != null);
        if (NativeErrorDescription != null) {
            info.AddValue("NativeErrorDescription", NativeErrorDescription);
        }
        base.GetObjectData(info, context);
    }

    #endregion
}
