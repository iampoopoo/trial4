namespace Glekcraft.Config;

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// The root configuration for the entire game.
/// </summary>
[JsonObject(
    Title = "Glekcraft Root Configuration",
    Description = "The root configuration for the entire game.",
    ItemNullValueHandling = NullValueHandling.Ignore,
    MissingMemberHandling = MissingMemberHandling.Ignore,
    MemberSerialization = MemberSerialization.OptIn,
    ItemRequired = Required.Default,
    NamingStrategyType = typeof(CamelCaseNamingStrategy),
    ItemReferenceLoopHandling = ReferenceLoopHandling.Ignore
)]
public class RootConfig {
    #region Public Static Methods

    /// <summary>
    /// Load a root config from the given path to a configuration file.
    /// </summary>
    /// <param name="path">
    /// The path to the configuration file to load from.
    /// </param>
    /// <returns>
    /// The root configuration that was loaded.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the path points to a configuration file that does not exist.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if the configuration file could not be deserialized.
    /// </exception>
    public static RootConfig Load(string path) {
        if (!File.Exists(path)) {
            throw new FileNotFoundException("No such configuration file exists", path);
        }
        var result = JsonConvert.DeserializeObject<RootConfig>(File.ReadAllText(path));
        if (result == null) {
            throw new JsonException("Failed to deserialize configuration file");
        }
        return result;
    }

    /// <summary>
    /// Load a root config from the given path to a configuration file or return
    /// the default configuration if the file could not be loaded.
    /// </summary>
    /// <param name="path">
    /// The path to the configuration file to load from.
    /// </param>
    /// <returns>
    /// The root configuration that was loaded or the default root configuration
    /// if the file could not be loaded.
    /// </returns>
    public static RootConfig LoadOrDefault(string path) {
        try {
            return Load(path);
        } catch {
            return new RootConfig();
        }
    }

    /// <summary>
    /// Save the given root configuration to the given path.
    /// </summary>
    /// <param name="path">
    /// The path to write the root configuration to.
    /// </param>
    /// <param name="config">
    /// The root configuration to write.
    /// </param>
    /// <exception cref="IOException">
    /// Thrown if the path points to a directory.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown if the configuration directory could not be created.
    /// </exception>
    public static void Save(string path, RootConfig config) {
        if (Directory.Exists(path)) {
            throw new IOException("Path is a directory");
        }
        var dirName = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(dirName) && !Directory.Exists(dirName)) {
            var dirInfo = Directory.CreateDirectory(dirName);
            if (!dirInfo.Exists) {
                throw new IOException("Failed to create configuration directory");
            }
        }
        using var stream = File.Open(path, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream);
        using var jsonWriter = new JsonTextWriter(writer);
        jsonWriter.Indentation = 4;
        jsonWriter.IndentChar = ' ';
        jsonWriter.Formatting = Formatting.Indented;
        var serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, config);
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// The configuration for the game window.
    /// </summary>
    [JsonProperty()]
    public WindowConfig Window { get; set; } = new WindowConfig();

    /// <summary>
    /// The configuration for the game graphics.
    /// </summary>
    [JsonProperty()]
    public GraphicsConfig Graphics { get; set; } = new GraphicsConfig();

    #endregion
}
