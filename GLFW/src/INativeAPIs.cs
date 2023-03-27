namespace Glekcraft.GLFW;

/// <summary>
/// An interface for classes that provide access to native GLFW APIs.
/// </summary>
public interface INativeAPIs {
    /// <summary>
    /// A delegate for methods that can be called when an error occurs in the
    /// native library.
    /// </summary>
    /// <param name="error">
    /// The error code for the error that occurred.
    /// </param>
    /// <param name="description">
    /// The description of the error that occurred.
    /// </param>
    delegate void ErrorCallback(int error, string? description);

    /// <summary>
    /// Get the version for the native library.
    /// </summary>
    /// <returns>
    /// The version for the native library.
    /// </returns>
    Version GetVersion();

    /// <summary>
    /// Get the version string for the native library.
    /// </summary>
    /// <returns>
    /// The version string for the native library.
    /// </returns>
    string GetVersionString();

    /// <summary>
    /// Set an initialization hint for the native library.
    /// </summary>
    /// <param name="hint">
    /// The hint to set.
    /// </param>
    /// <param name="value">
    /// The value to set the hint to.
    /// </param>
    void InitHint(int hint, int value);

    /// <summary>
    /// Set an initialization hint for the native library.
    /// </summary>
    /// <param name="hint">
    /// The hint to set.
    /// </param>
    /// <param name="value">
    /// The value to set the hint to.
    /// </param>
    void InitHint(int hint, bool value);

    /// <summary>
    /// Initialize the native library.
    /// </summary>
    /// <returns>
    /// Whether native library initialized successfully.
    /// </returns>
    bool Init();

    /// <summary>
    /// Shut down the native library.
    /// </summary>
    void Terminate();

    /// <summary>
    /// Get the error code and description for the last error that occurred.
    /// </summary>
    /// <param name="description">
    /// The description of the last error that occurred.
    /// </param>
    /// <returns>
    /// The error code for the last error that occurred.
    /// </returns>
    int GetError(out string? description);

    /// <summary>
    /// Set the method to call when an error occurs in the native library.
    /// </summary>
    /// <param name="callback">
    /// The method to call when an error occurs in the native library.
    /// </param>
    /// <returns>
    /// The previous method to call when an error occurs in the native library,
    /// if one was set.
    /// </returns>
    ErrorCallback? SetErrorCallback(ErrorCallback? callback);
}
