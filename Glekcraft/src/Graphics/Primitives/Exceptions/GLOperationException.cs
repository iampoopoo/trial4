namespace Glekcraft.Graphics.Primitives.Exceptions;

using System.Runtime.Serialization;

using Silk.NET.OpenGL;

/// <summary>
/// An exception that is thrown when an OpenGL operation fails.
/// </summary>
[Serializable]
public class GLOperationException : Exception {
    #region Public Properties

    /// <summary>
    /// The error code returned by the failed operation.
    /// </summary>
    public ErrorCode ErrorCode { get; }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    public GLOperationException(ErrorCode errorCode) : base($"OpenGL operation failed: {errorCode}") =>
        ErrorCode = errorCode;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    public GLOperationException(ErrorCode errorCode, string? message) : base(message) =>
        ErrorCode = errorCode;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    /// <param name="inner">
    /// The inner exception.
    /// </param>
    public GLOperationException(ErrorCode errorCode, string? message, Exception? inner) : base(message, inner) =>
        ErrorCode = errorCode;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="info">
    /// The serialization information.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    protected GLOperationException(SerializationInfo info, StreamingContext context) : base(info, context) =>
        ErrorCode = (ErrorCode?)info.GetValue(nameof(ErrorCode), typeof(ErrorCode)) ?? throw new ArgumentNullException(nameof(info), $"SerializationInfo did not contain a value for field {nameof(ErrorCode)}");

    #endregion

    #region Public Methods

    /// <summary>
    /// Get the object data.
    /// </summary>
    /// <param name="info">
    /// The serialization information.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue(nameof(ErrorCode), ErrorCode, typeof(ErrorCode));
        base.GetObjectData(info, context);
    }

    #endregion
}
