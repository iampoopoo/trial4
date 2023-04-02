namespace Glekcraft.GLFW;

/// <summary>
/// An enumeration of possible error codes the native library can raise.
/// </summary>
public enum ErrorCode {
    /// <summary>
    /// The default error code for no errors.
    /// </summary>
    NoError = 0,

    /// <summary>
    /// The native library is not initialized.
    /// </summary>
    NotInitialized = 0x00010001,

    /// <summary>
    /// There is no OpenGL/Vulkan context active on the current thread.
    /// </summary>
    NoCurrentContext = 0x00010002,

    /// <summary>
    /// The enumeration value was invalid.
    /// </summary>
    InvalidEnum = 0x00010003,

    /// <summary>
    /// The value was invalid.
    /// </summary>
    InvalidValue = 0x00010004,

    /// <summary>
    /// A memory allocation failed.
    /// </summary>
    OutOfMemory = 0x00010005,

    /// <summary>
    /// The requested graphics API is not available.
    /// </summary>
    ApiUnavailable = 0x00010006,

    /// <summary>
    /// The request graphics API version is not available.
    /// </summary>
    VersionUnavailable = 0x00010007,

    /// <summary>
    /// A generic operating system error occurred.
    /// </summary>
    PlatformError = 0x00010008,

    /// <summary>
    /// The requested framebuffer format is not available.
    /// </summary>
    FormatUnavailable = 0x00010009,

    /// <summary>
    /// The window has no graphics context.
    /// </summary>
    NoWindowContext = 0x0001000A,
}
