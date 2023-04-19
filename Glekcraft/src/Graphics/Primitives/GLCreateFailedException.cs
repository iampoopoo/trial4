namespace Glekcraft.Graphics.Primitives;

using System.Runtime.Serialization;

/// <summary>
/// An exception that represents the OpenGL rendering context returning an error
/// from the <c>glGetError</c> function.
/// </summary>
[Serializable]
public class GLCreateFailedException : Exception {
    #region Public Properties

    /// <summary>
    /// The error code returned from the OpenGL rendering context.
    /// </summary>
    public string ResourceType { get; }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="resourceType">
    /// The error code returned from the OpenGL rendering context.
    /// </param>
    public GLCreateFailedException(string resourceType) : base($"Failed to create OpenGL resource: {resourceType}") =>
        ResourceType = resourceType;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="resourceType">
    /// The error code returned from the OpenGL rendering context.
    /// </param>
    /// <param name="message">
    /// A message describing what went wrong.
    /// </param>
    public GLCreateFailedException(string resourceType, string? message) : base(message) =>
        ResourceType = resourceType;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="resourceType">
    /// The error code returned from the OpenGL rendering context.
    /// </param>
    /// <param name="message">
    /// A message describing what went wrong.
    /// </param>
    /// <param name="inner">
    /// The inner exception.
    /// </param>
    public GLCreateFailedException(string resourceType, string? message, Exception? inner) : base(message, inner) =>
        ResourceType = resourceType;

    /// <summary>
    /// The deserialization constructor.
    /// </summary>
    /// <param name="info">
    /// The serialization info.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    protected GLCreateFailedException(SerializationInfo info, StreamingContext context) : base(info, context) =>
        ResourceType = info.GetString("ResourceType") ?? throw new ArgumentNullException(nameof(info), "SerializationInfo does not contain a value for ResourceType");

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
        info.AddValue("ResourceType", ResourceType);
        base.GetObjectData(info, context);
    }

    #endregion
}
