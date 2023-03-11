namespace Glekcraft.Config;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// The configuration for the game window.
/// </summary>
[JsonObject(
    Title = "Glekcraft Window Configuration",
    Description = "The configuration for the game window.",
    ItemNullValueHandling = NullValueHandling.Ignore,
    MissingMemberHandling = MissingMemberHandling.Ignore,
    MemberSerialization = MemberSerialization.OptIn,
    ItemRequired = Required.Default,
    NamingStrategyType = typeof(CamelCaseNamingStrategy),
    ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore
)]
public class WindowConfig {
    #region Public Properties

    /// <summary>
    /// The width of the window.
    /// </summary>
    [JsonProperty()]
    public int Width { get; set; } = 1280;

    /// <summary>
    /// The height of the window.
    /// </summary>
    [JsonProperty()]
    public int Height { get; set; } = 720;

    /// <summary>
    /// Whether to use fullscreen mode.
    /// </summary>
    [JsonProperty()]
    public bool Fullscreen { get; set; } = false;

    #endregion
}
