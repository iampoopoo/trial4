namespace Glekcraft;

using System.Drawing;

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using Glekcraft.Config;

/// <summary>
/// The main game state/logic controller.
/// </summary>
public sealed class Game : IDisposable {
    #region Public Properties

    public RootConfig? Config {
        get;
        private set;
    }

    /// <summary>
    /// The main game window.
    /// </summary>
    public IWindow? Window {
        get;
        private set;
    }

    /// <summary>
    /// The main input context.
    /// </summary>
    public IInputContext? Input {
        get;
        private set;
    }

    /// <summary>
    /// The main GPU context.
    /// </summary>
    public GL? GraphicsContext {
        get;
        private set;
    }

    /// <summary>
    /// Whether this instance has been disposed.
    /// </summary>
    public bool IsDisposed {
        get;
        private set;
    }

    #endregion

    #region Public Constructors/Finalizer

    /// <summary>
    /// Create a new instance.
    /// </summary>
    public Game() {
        //-- Does nothing
    }

    /// <summary>
    /// The finalizer.
    /// </summary>
    ~Game() =>
        Dispose(false);

    #endregion

    #region Public Methods

    /// <summary>
    /// Initialize the game.
    /// </summary>
    public void Init() {
        Config = RootConfig.LoadOrDefault("config.json");
        //-- Immediately overwrite the config in case we loaded the defaults
        RootConfig.Save("config.json", Config);
        var glOpts = GraphicsAPI.Default;
        glOpts.Version = new(4, 5);
        glOpts.Profile = ContextProfile.Core;
        glOpts.Flags = ContextFlags.ForwardCompatible;
        var windowOpts = WindowOptions.Default;
        windowOpts.API = glOpts;
        windowOpts.Size = new(Config.Window.Width, Config.Window.Height);
        windowOpts.Title = "Glekcraft";
        windowOpts.WindowClass = "Glekcraft";
        windowOpts.IsVisible = false;
        windowOpts.WindowState = WindowState.Normal;
        windowOpts.WindowBorder = WindowBorder.Resizable;
        windowOpts.VSync = Config.Graphics.VerticalSync;
        windowOpts.Samples = Config.Graphics.AntiAliasingSamples;
        windowOpts.FramesPerSecond = Config.Graphics.MaxFramerate;
        windowOpts.ShouldSwapAutomatically = false;
        windowOpts.UpdatesPerSecond = 60;
        Window = Silk.NET.Windowing.Window.Create(windowOpts);
        Window.Load += OnWindowLoad;
        Window.Update += OnWindowUpdate;
        Window.Render += OnWindowRender;
        Window.Closing += OnWindowClosing;
    }

    /// <summary>
    /// Run the game.
    /// </summary>
    public void Run() =>
        Window?.Run();

    /// <summary>
    /// Dispose this instance and the resources its managing.
    /// </summary>
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Private Methods

    private void OnWindowLoad() {
        if (Window == null) {
            throw new InvalidOperationException("Main game window does not exist");
        }
        Input = Window.CreateInput();
        GraphicsContext = Window.CreateOpenGL();
        if (Config?.Window.Fullscreen ?? false) {
            Window.WindowState = WindowState.Fullscreen;
        }
        Window.Center();
        Window.IsVisible = true;
    }

    private void OnWindowUpdate(double delta) {
        var kbs = Input?.Keyboards ?? Array.Empty<IKeyboard>();
        foreach (var kb in kbs) {
            if (kb.IsKeyPressed(Key.Escape)) {
                Window?.Close();
            }
        }
    }

    private void OnWindowRender(double delta) {
        var fbSize = Window?.FramebufferSize;
        if (fbSize != null) {
            GraphicsContext?.Viewport(fbSize.Value);
        }
        GraphicsContext?.ClearColor(Color.CornflowerBlue);
        GraphicsContext?.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        // TODO: Render
        Window?.SwapBuffers();
    }

    private void OnWindowClosing() {
        GraphicsContext?.Dispose();
        GraphicsContext = null;
        Input?.Dispose();
        Input = null;
    }

    /// <summary>
    /// Dispose this instance and the resources its managing.
    /// </summary>
    /// <param name="managed">
    /// Whether this method is being called from managed code or from unmanaged
    /// code (e.g. the garbage collector).
    /// </param>
    private void Dispose(bool managed) {
        if (IsDisposed) {
            return;
        }
        if (managed) {
            Window?.Dispose();
            Window = null;
        }
        // TODO
        IsDisposed = true;
    }

    #endregion
}
