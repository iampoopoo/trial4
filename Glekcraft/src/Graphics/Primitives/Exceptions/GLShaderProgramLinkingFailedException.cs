namespace Glekcraft.Graphics.Primitives.Exceptions;

using System.Runtime.Serialization;

/// <summary>
/// An exception that is thrown when an OpenGL shader program fails to link.
/// </summary>
[Serializable]
public class GLShaderProgramLinkingFailedException : Exception {
    #region Public Properties

    /// <summary>
    /// The information log from the shader program linkage attempt.
    /// </summary>
    public string? InfoLog { get; }

    #endregion

    #region Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="infoLog">
    /// The information log from the shader program linkage attempt.
    /// </param>
    public GLShaderProgramLinkingFailedException(string? infoLog) : base($"OpenGL shader program linkage failed: {infoLog}") =>
        InfoLog = infoLog;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="infoLog">
    /// The information log from the shader program linkage attempt.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    public GLShaderProgramLinkingFailedException(string? infoLog, string? message) : base(message) =>
        InfoLog = infoLog;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="infoLog">
    /// The information log from the shader program linkage attempt.
    /// </param>
    /// <param name="message">
    /// The message to include in the exception.
    /// </param>
    /// <param name="inner">
    /// The inner exception.
    /// </param>
    public GLShaderProgramLinkingFailedException(string? infoLog, string? message, Exception? inner) : base(message, inner) =>
        InfoLog = infoLog;

    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="info">
    /// The serialization information.
    /// </param>
    /// <param name="context">
    /// The streaming context.
    /// </param>
    protected GLShaderProgramLinkingFailedException(SerializationInfo info, StreamingContext context) : base(info, context) =>
        InfoLog = info.GetString(nameof(InfoLog));

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
        info.AddValue(nameof(InfoLog), InfoLog);
        base.GetObjectData(info, context);
    }

    #endregion
}
