namespace Glekcraft.Graphics.Primitives;

using System.Runtime.Serialization;

using Silk.NET.OpenGL;

/// <summary>
/// An exception that represents the OpenGL rendering context returning an error
/// from the <c>glGetError</c> function.
/// </summary>
[Serializable]
public class GLException : Exception {
    #region Public Properties

    /// <summary>
    /// The error code returned from the OpenGL rendering context.
    /// </summary>
    public ErrorCode ErrorCode { get; }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned from the OpenGL rendering context.
    /// </param>
    public GLException(ErrorCode errorCode) : base($"OpenGL error: {errorCode}") =>
        ErrorCode = errorCode;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned from the OpenGL rendering context.
    /// </param>
    /// <param name="message">
    /// A message describing what went wrong.
    /// </param>
    public GLException(ErrorCode errorCode, string? message) : base(message) =>
        ErrorCode = errorCode;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned from the OpenGL rendering context.
    /// </param>
    /// <param name="message">
    /// A message describing what went wrong.
    /// </param>
    /// <param name="inner">
    /// The inner exception.
    /// </param>
    public GLException(ErrorCode errorCode, string? message, Exception? inner) : base(message, inner) =>
        ErrorCode = errorCode;

    /// <summary>
    /// The deserialization constructor.
    /// </summary>
    /// <param name="info">
    /// The serialization info.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    protected GLException(SerializationInfo info, StreamingContext context) : base(info, context) =>
        ErrorCode = (ErrorCode)(info.GetValue("ErrorCode", typeof(ErrorCode)) ?? throw new ArgumentNullException(nameof(info), "SerializationInfo does not contain a value for ErrorCode"));

    #endregion

    #region Public Methods

    /// <summary>
    /// Serialize the exception.
    /// </summary>
    /// <param name="info">
    /// The serialization info.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("ErrorCode", ErrorCode, typeof(ErrorCode));
        base.GetObjectData(info, context);
    }

    #endregion
}
