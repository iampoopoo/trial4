namespace Glekcraft.Config;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// The configuration for the game window.
/// </summary>
[JsonObject(
    Title = "Glekcraft Graphics Configuration",
    Description = "The configuration for the game graphics.",
    ItemNullValueHandling = NullValueHandling.Ignore,
    MissingMemberHandling = MissingMemberHandling.Ignore,
    MemberSerialization = MemberSerialization.OptIn,
    ItemRequired = Required.Default,
    NamingStrategyType = typeof(CamelCaseNamingStrategy),
    ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore
)]
public class GraphicsConfig {
    #region Public Properties

    /// <summary>
    /// The number of anti-aliasing samples to use.
    /// </summary>
    [JsonProperty()]
    public int AntiAliasingSamples { get; set; } = 0;

    /// <summary>
    /// Whether to use vertical sync.
    /// </summary>
    [JsonProperty()]
    public bool VerticalSync { get; set; } = false;

    /// <summary>
    /// The maximum framerate.
    /// </summary>
    [JsonProperty()]
    public int MaxFramerate { get; set; } = 60;

    #endregion
}
