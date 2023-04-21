namespace Glekcraft.Graphics.Primitives.Exceptions;

using System.Runtime.Serialization;

using Silk.NET.OpenGL;

/// <summary>
/// An exception that is thrown when an OpenGL object fails to be created
/// </summary>
[Serializable]
public class GLObjectCreationFailedException : Exception {
    #region Public Properties

    /// <summary>
    /// The error code returned by the failed operation.
    /// </summary>
    public ErrorCode? ErrorCode { get; }

    /// <summary>
    /// The type of OpenGL object that failed to be created.
    /// </summary>
    public GLObjectType ObjectType { get; }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    /// <param name="objectType">
    /// The type of OpenGL object that failed to be created.
    /// </param>
    public GLObjectCreationFailedException(GLObjectType objectType) : base($"OpenGL object creation failed: {objectType}") =>
        ObjectType = objectType;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="objectType">
    /// The type of OpenGL object that failed to be created.
    /// </param>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    public GLObjectCreationFailedException(GLObjectType objectType, ErrorCode? errorCode) : base($"OpenGL object creation failed: {objectType} ({errorCode})") {
        ErrorCode = errorCode;
        ObjectType = objectType;
    }

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="objectType">
    /// The type of OpenGL object that failed to be created.
    /// </param>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    public GLObjectCreationFailedException(GLObjectType objectType, ErrorCode? errorCode, string? message) : base(message) {
        ErrorCode = errorCode;
        ObjectType = objectType;
    }

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="objectType">
    /// The type of OpenGL object that failed to be created.
    /// </param>
    /// <param name="errorCode">
    /// The error code returned by the failed operation.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    /// <param name="inner">
    /// The inner exception.
    /// </param>
    public GLObjectCreationFailedException(GLObjectType objectType, ErrorCode errorCode, string? message, Exception? inner) : base(message, inner) {
        ErrorCode = errorCode;
        ObjectType = objectType;
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
    protected GLObjectCreationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) {
        ErrorCode = (ErrorCode?)info.GetValue(nameof(ErrorCode), typeof(ErrorCode)) ?? throw new ArgumentNullException(nameof(info), $"SerializationInfo did not contain a value for field {nameof(ErrorCode)}");
        ObjectType = (GLObjectType?)info.GetValue(nameof(ObjectType), typeof(GLObjectType)) ?? throw new ArgumentNullException(nameof(info), $"SerializationInfo did not contain a value for field {nameof(ObjectType)}");
    }

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
        info.AddValue(nameof(ObjectType), ObjectType, typeof(GLObjectType));
        base.GetObjectData(info, context);
    }

    #endregion
}
