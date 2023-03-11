namespace Glekcraft;

using Silk.NET.Maths;

using Newtonsoft.Json;

/// <summary>
/// The game configuration.
/// </summary>
[JsonObject(
    Title = "Glekcraft Configuration",
    Description = "The configuration for the Glekcraft game.",
    MemberSerialization = MemberSerialization.OptIn,
    ItemRequired = Required.Default,
    ItemNullValueHandling = NullValueHandling.Ignore,
    MissingMemberHandling = MissingMemberHandling.Ignore
)]
public class Config {
    #region Public Static Properties

    public static Config DEFAULT {
        get;
    }

    #endregion

    #region Public Properties

    [JsonProperty("windowWidth")]
    public int WindowWidth {
        get;
        set;
    }

    [JsonProperty("windowHeight")]
    public int WindowHeight {
        get;
        set;
    }

    public Vector2D<int> WindowSize {
        get => new(WindowWidth, WindowHeight);
        set {
            WindowWidth = value.X;
            WindowHeight = value.Y;
        }
    }

    [JsonProperty("vsyncEnabled")]
    public bool VSyncEnabled {
        get;
        set;
    }

    [JsonProperty("maxFramerate")]
    public int MaxFramerate {
        get;
        set;
    }

    [JsonProperty("antialiasSamples")]
    public int AntialiasSamples {
        get;
        set;
    }

    #endregion

    #region Constructors/Finalizer

    static Config() =>
        DEFAULT = new();

    public Config() {
        WindowWidth = 640;
        WindowHeight = 480;
        VSyncEnabled = false;
        MaxFramerate = 60;
        AntialiasSamples = 0;
    }

    #endregion
}
